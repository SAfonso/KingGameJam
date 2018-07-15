using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    //public GameObject cube;

    [SerializeField]
    private GameObject _charge;
    [SerializeField]
    private SpriteRenderer[] _chargeLevels;

    private float _chargeDistance;
    private Ray _clickPoint;
    private Ray _holdPoint;
    private Vector3 _firstPoint;
    private Transform _releasePointTransform;
    private bool _isAiming = false;
    private bool _shoot = false;
    private float _falseForce;

    private GameObject _player;
    private Vector3 _finalPosition;

    [SerializeField]
    private RotationPlanet _sun;

    private Vector3 _velocity = Vector3.zero;
    private float _smooth = 0.4f;
    public bool canTouch = true;

    public bool Shoot
    {
        set { _shoot = value; }
        get { return _shoot; }
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        foreach (var level in _chargeLevels)
        {
            level.enabled = false;
        }
    }

    private void Update()
    {
        _finalPosition = _chargeLevels[4].transform.position; // Should be distance / 10

        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        //if (Input.mousePosition.y < (Screen.height / 2) - 1)
        //{
            if (canTouch)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ClickOnPlanet();
                }

                if (Input.GetMouseButton(0) && _isAiming)
                {
                    _falseForce = HoldOnPlanet();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    Release();
                    Camera.main.GetComponent<CameraController>().ZoomIn = true;
                    Camera.main.GetComponent<CameraController>().ZoomOut = false;
                }
            }
        //}

        if (_shoot)
            TakeOff();
    }

    private void ClickOnPlanet()
    {
        _clickPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        _firstPoint = Input.mousePosition;
        RaycastHit hit;

        if (Physics.Raycast(_clickPoint, out hit, 50))
        {
            if (hit.collider.tag == "ActivePlanet")
            {
                //foreach (var level in _chargeLevels)
                //{
                //    level.GetComponent<Animator>().SetBool("Hide", false);
                //}
                _isAiming = true;
                _charge.transform.position = hit.collider.transform.position;
            }
        }
        else
        {
            Camera.main.GetComponent<CameraController>().ZoomOut = true;
            Camera.main.GetComponent<CameraController>().ZoomIn = false;
        }
    }

    private float HoldOnPlanet()
    {
        RaycastHit hit2;
        int layerMask = 1 << 9;

        _holdPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        float currentDistance = Vector3.Distance(Input.mousePosition, _firstPoint);

        Debug.DrawRay(_chargeLevels[4].transform.position, Vector3.up * 1000, Color.red, 2);
        if (Physics.Raycast(_chargeLevels[4].transform.position, Vector3.forward, out hit2, Mathf.Infinity))
        {
            Debug.Log(hit2.collider.name);
        }

        //Debug.LogFormat("First point is {0} and hold point is {1}. Distance is {2}", _firstPoint, _holdPoint, currentDistance);
        _charge.transform.rotation = Quaternion.LookRotation(Vector3.forward, _holdPoint.origin);
        CheckChargeLevels(currentDistance);
        return (currentDistance);
    }

    private void Release()
    {
        //if (_sun.AllowLand)
            _shoot = true;

        _isAiming = false;

        canTouch = false;

        foreach (var level in _chargeLevels)
        {
            level.enabled = false;

            //level.GetComponent<Animator>().enabled = true;
            //level.GetComponent<Animator>().SetBool("Hide", true);
        }
    }

    private void TakeOff()
    {
        float smoothDamp = 1 / ((_falseForce * _smooth) / 10);
        //Debug.Log(smoothDamp);
        _player.transform.position = Vector3.SmoothDamp(_player.transform.position, _finalPosition, ref _velocity, smoothDamp);
    }

    private void CheckChargeLevels(float currentDistance)
    {
        if (currentDistance <= 30)
        {
            _chargeLevels[0].enabled = true;
            _chargeLevels[1].enabled = false;
            _chargeLevels[2].enabled = false;
            _chargeLevels[3].enabled = false;
            _chargeLevels[4].enabled = false;

            //var newColor = GetAlpha(1, 0, currentDistance);
            //_chargeLevels[0].GetComponent<Animator>().enabled = false;
            //_chargeLevels[0].color = newColor;
        }
        else if (currentDistance > 30 && currentDistance <= 60)
        {
            _chargeLevels[1].enabled = true;
            _chargeLevels[2].enabled = false;
            _chargeLevels[3].enabled = false;
            _chargeLevels[4].enabled = false;

            //var newColor = GetAlpha(2, 1, currentDistance);
            //_chargeLevels[1].GetComponent<Animator>().enabled = false;
            //_chargeLevels[1].color = newColor;
        }
        else if (currentDistance > 60 && currentDistance <= 90)
        {
            _chargeLevels[2].enabled = true;
            _chargeLevels[3].enabled = false;
            _chargeLevels[4].enabled = false;

            //var newColor = GetAlpha(3, 2, currentDistance);
            //_chargeLevels[2].GetComponent<Animator>().enabled = false;
            //_chargeLevels[2].color = newColor;
        }
        else if (currentDistance > 90 && currentDistance <= 120)
        {
            _chargeLevels[3].enabled = true;
            _chargeLevels[4].enabled = false;

            //var newColor = GetAlpha(4, 3, currentDistance);
            //_chargeLevels[3].GetComponent<Animator>().enabled = false;
            //_chargeLevels[3].color = newColor;
        }
        else if (currentDistance > 120)
        {
            _chargeLevels[4].enabled = true;

            //var newColor = GetAlpha(4, 3, totalDistance);
            //_chargeLevels[4].GetComponent<Animator>().enabled = false;
            //_chargeLevels[4].color = newColor;
        }
    }

    private Color GetAlpha(int nextLevel, int currentLevel, float currentDistance)
    {
        float midDistance;
        float maxDistance;

        midDistance = Vector3.Distance(_chargeLevels[nextLevel].transform.position, _chargeLevels[currentLevel].transform.position);
        maxDistance = Vector3.Distance(_chargeLevels[0].transform.position, _chargeLevels[nextLevel].transform.position);

        var alphaValue = currentDistance - (maxDistance - midDistance) / midDistance;
        Debug.Log("alphaValue = distance - (maxDistance - midDistance) / midDistance");
        Debug.LogFormat("{4} = {0} - ({1} - {2}) / {3}", currentDistance, maxDistance, midDistance, midDistance, alphaValue);
        Color currentColor = _chargeLevels[currentLevel].color;
        Color color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Clamp01(alphaValue));

        //Debug.LogFormat("Changing level {0}: {1}", currentLevel, Mathf.Clamp01(alphaValue));

        return color;
    }
}
