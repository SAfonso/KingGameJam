using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPlanet : MonoBehaviour 
{

    public Transform center;
    private Vector3  axis = Vector3.back;

    public float orbitRadius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float translationSpeed = 80.0f;

    public bool isSun = false;

    [SerializeField]
    private float planetMass = 1f;

    private bool wasVisited = false;
    private bool _allowLand = false;

    public GameObject hierva;

    
    public Vector3 rotationSpeed = new Vector3 (0f, 1f, 0f);

    private Transform landSpace;
    private Transform planet;

	//public Vector3 distance = Vector3.zero;

    void Start() 
    {
        planet = transform.GetChild(0);
        landSpace = transform.GetChild(1);

        if (center != null)
            transform.position = (transform.position - center.position).normalized * orbitRadius + center.position;
        
        if (!isSun)
            AllowLand = true;

    }


    public GameObject GetHierva()
    {
        return hierva;
    }
    
    void Update() 
    {
        if (center != null)
        {
            //Translation of the planet arround the sun
            transform.RotateAround(center.position, axis, translationSpeed * Time.deltaTime);
            var desiredPosition = (transform.position - center.position).normalized * orbitRadius + center.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
			//distance = (transform.position - center.position).normalized;

        }

        // Vector3 relativePos = center.position - landSpace.transform.position;
        // Quaternion rotation = Quaternion.LookRotation(relativePos);
        // landSpace.transform.rotation = rotation;

        //Rotate of the planet
        planet.transform.Rotate(rotationSpeed.x * Time.deltaTime, rotationSpeed.y * Time.deltaTime, rotationSpeed.z * Time.deltaTime);
    }

    public Transform GetLandSpace()
    {
        return landSpace;
    }

    public float GetPlanetMass()
    {
        return planetMass;
    }

    public bool PlanetWasVisited
    {
        set { wasVisited = value; }
        get { return wasVisited; }
    }

    public bool AllowLand
    {
        set { _allowLand = value; }
        get { return _allowLand; }
    }
}
