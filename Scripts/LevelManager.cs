using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	private GameObject[] planets;
	private RotationPlanet _sun;
	private CameraController myCam;
	private int numberOfPlanets = 0;
	private int numberOfPlanetsVisited = 0;

	public int _level = 1;

	int nextLevel = 0;


	// Use this for initialization
	void Start () {
		planets = GameObject.FindGameObjectsWithTag("Planet");
		numberOfPlanets = planets.Length;

		_sun = GameObject.FindGameObjectWithTag("Sun").GetComponent<RotationPlanet>();

		numberOfPlanetsVisited = 0;	
		myCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
	}

	public void PlanetVisited()
	{
		numberOfPlanetsVisited++;
		if(CheckSolarSystem())
			ActiveSun();
	}

	public bool CheckSolarSystem()
	{
		bool value = false;

		if (numberOfPlanetsVisited == numberOfPlanets)
		 	value = true;

		return value;
	}

	public void ActiveSun()
	{
		_sun.AllowLand = true;
	}

    public int ThisLevel
    {
        set { _level = value; }
        get { return _level; }
    }

	public void FinishLevel()
	{
		nextLevel = _level + 1;
		GameManager.INSTANCE.GameLevel = nextLevel;
		myCam.ZoomIn = false;
		myCam.ZoomOut = true;
		if (GameManager.INSTANCE.GameLevel <= 3) {
			StartCoroutine ("WaitUntilLoad");
		} else 
		{
			SceneManager.LoadScene(0);
		}
			
		
	}


	IEnumerator WaitUntilLoad() {
   	    yield return new WaitForSeconds(7f);
		SceneManager.LoadScene(nextLevel);
	}
	
}
