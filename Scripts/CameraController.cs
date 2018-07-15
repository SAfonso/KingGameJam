using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour {

    //public float initialSize;
    //public float zoomOutSize;

	//public bool increase = false;

	//public GameObject myCam;
    private GameObject _target;       //Public variable to store a reference to the player game object
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _sun;
    	
	//public float actualCameraSize;
	private Vector3 offset;

    private float _initialCameraSize = 1.7f;
    [SerializeField]
    private float _finalCameraSize = 4;
    [SerializeField]
    private Transform _sunPosition;
    [SerializeField]
    private float _smoothDamp = 0.4f;
    private float _velocitySize = 0;
    private float _velocityAlpha = 0;
    private Vector3 _velocityPos = Vector3.zero;
    private Transform _initialCameraTransform;

    [SerializeField]
    private Sprite[] _orbitThin;
    [SerializeField]
    private Sprite[] _orbitThick;
    [SerializeField]
    private GameObject[] _orbits;

    private bool _zoomOut = false;
    private bool _zoomIn = false;

    public bool ZoomOut
    {
        set { _zoomOut = value; }
    }

    public bool ZoomIn
    {
        set { _zoomIn = value; }
    }

 	//void IncreaseFOV()
  //  {
  //      //check that current FOV is different than Zoomed
  //      if (actualCameraSize != zoomOutSize)
  //      {
			
  //          //check if current FOV is grater than the Zoomed in FOV input and increment the FOV smoothly
  //          if (actualCameraSize < zoomOutSize)
  //          {
  //              myCam.GetComponent<Camera>().DOOrthoSize(zoomOutSize, 1f).OnComplete(() => { _target = _sun; });
  //              RecalculateActualSize();
  //          }
  //          else
  //          {
  //              //then current FOV gets to the same or smaller value than the Zoomed in input
  //              //set FOV as the Zoomed in input
  //              if (actualCameraSize >= zoomOutSize)
  //              {
  //                  Camera.main.orthographicSize = zoomOutSize;
  //              }
  //          }
  //      }
  //  }

 	//void DecreaseFOV()
  //  {
  //      //check that current FOV is different than Zoomed
  //      if (actualCameraSize != initialSize)
  //      {
		//	if (actualCameraSize > initialSize)
  //          {
		//		myCam.GetComponent<Camera>().DOOrthoSize(initialSize, 1f).OnComplete(() => { _target = _player; });
		//		RecalculateActualSize();
  //          }
  //          else
  //          {
  //              if (actualCameraSize <= initialSize)
  //              {
  //                  Camera.main.orthographicSize = initialSize;
  //              }
  //          }
  //      }
  //  }

	private void Start()
    {
        // Thick orbits
        for (int i = 0; i < _orbits.Length; i++)
        {
            //_orbits[i].GetComponent<SpriteRenderer>().sprite = _orbitThin[i];
        }

        _target = _player;
        _initialCameraTransform = Camera.main.transform;

        // Get the ortografic size
        //RecalculateActualSize();

		// Initialize the following camera
		offset = transform.position - _target.transform.position;
	}

	//private void RecalculateActualSize()
 //   {
	//	actualCameraSize = myCam.GetComponent<Camera>().orthographicSize;
	//}

	private void Update()
    {
        if (_zoomIn)
            OnZoomIn();
        else if (_zoomOut)
            OnZoomOut();
        else
            transform.position = _target.transform.position + offset;
    }

    public void OnZoomOut()
    {
        Vector3 finalPosition;
        Color color = _orbits[0].GetComponent<SpriteRenderer>().color;
        float alpha = color.a;
        finalPosition = new Vector3(_sun.transform.position.x, _sun.transform.position.y, _initialCameraTransform.position.z);

        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, _finalCameraSize, ref _velocitySize, _smoothDamp);
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, finalPosition, ref _velocityPos, _smoothDamp);
        foreach (var orbit in _orbits)
        {
            orbit.GetComponent<SpriteRenderer>().color = new Color(
                orbit.GetComponent<SpriteRenderer>().color.r,
                orbit.GetComponent<SpriteRenderer>().color.g,
                orbit.GetComponent<SpriteRenderer>().color.b,
                Mathf.SmoothDamp(alpha, 0, ref _velocityAlpha, _smoothDamp));
        }
    }

    public void OnZoomIn()
    {
        Vector3 finalPosition;
        Color color = _orbits[0].GetComponent<SpriteRenderer>().color;
        float alpha = color.a;
        finalPosition = new Vector3(_player.transform.position.x, _player.transform.position.y, _initialCameraTransform.position.z);

        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, _initialCameraSize, ref _velocitySize, _smoothDamp);
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, finalPosition, ref _velocityPos, _smoothDamp);
        foreach (var orbit in _orbits)
        {
            orbit.GetComponent<SpriteRenderer>().color = new Color(
                orbit.GetComponent<SpriteRenderer>().color.r,
                orbit.GetComponent<SpriteRenderer>().color.g,
                orbit.GetComponent<SpriteRenderer>().color.b,
                Mathf.SmoothDamp(alpha, 1, ref _velocityAlpha, _smoothDamp));
        }
    }
}
