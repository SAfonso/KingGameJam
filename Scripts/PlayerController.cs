using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour 
{

	public Vector3  direction = Vector3.up;
	public float speed = 2f;
	public float lifeTimeInSpace = 3f;
	public float lifeTimeOnPlanet = 4f;
	private float lifeTimeCountDown;
	public bool move;

	

	[SerializeField]
	private InputController _imputManager;
	private LevelManager _levelManager;
	private bool isOnAPlanet = false;

	void Start () 
	{
		move = false;
		lifeTimeCountDown = lifeTimeInSpace;

		_levelManager = GameManager.FindObjectOfType<LevelManager>();
	}
	
	void Update () 
	{
		// Player is flying by the empty & deep space
		if (_imputManager.Shoot)
		{
			if(isOnAPlanet)
			{
				isOnAPlanet = false;
			}

			//transform.Translate(direction * speed * Time.deltaTime); // borrar esta linea cuando seañada el movimiento real
			lifeTimeCountDown -= Time.deltaTime;
			// Die inempty & deep space
			if(lifeTimeCountDown <= 0f)
			{
				//move = false;
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}

		// Player was land in a Planet
		// if(isOnAPlanet)
		// {
		// 	lifeTimeCountDown -= Time.deltaTime;
		// 	//Die on a planet
		// 	if(lifeTimeCountDown <= 0f)
		// 	{
		// 		Debug.Log("<color=red> YA ESTÁ MUERTO POR GILIPOLLAS!!!! </color>");
		// 	}

		// }
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Planet")
		{

			RotationPlanet planetCollide = other.transform.GetComponent<RotationPlanet>();
			GameObject planet = other.transform.gameObject;
			Transform land = planetCollide.GetLandSpace();

			planetCollide.GetHierva().SetActive(true);


			_imputManager.Shoot = false;
			//transform.position = Vector3.zero;
			// Copy the position &rotation of the landSpace
			transform.SetParent(planetCollide.transform);
			transform.position = planet.transform.position; // land.position;
			//transform.localRotation = land.rotation;
			
			_imputManager.canTouch = true;

			//move = false;
			// isOnAPlanet = true;
			// lifeTimeCountDown = lifeTimeOnPlanet;

			// If is the first time we land on thisplanet, add to the planet visited count
			if (!planetCollide.PlanetWasVisited)
			{
				planetCollide.PlanetWasVisited = true;
				_levelManager.PlanetVisited();
			}

			other.tag = "ActivePlanet";
			
		}
		else if (other.tag == "Asteroid")
		{
			//move = false; 
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			
		}
		else if(other.tag == "Sun")
		{
			//GameManager.INSTANCE.GameLevel = GameManager.INSTANCE.GameLevel+1;
			Debug.Log("<color=green> GANASTE!!!!! </color>");
			_imputManager.Shoot = false;
			transform.SetParent(other.gameObject.transform);
			transform.position = other.transform.position;
			other.gameObject.transform.parent.transform.GetChild(1).GetChild(0).GetComponent<Animator>().SetTrigger("Finish");
			_levelManager.FinishLevel();
		}

	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "ActivePlanet")
		{
			other.tag = "Planet";
		}
	}
}
