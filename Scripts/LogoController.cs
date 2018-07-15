using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
[CreateAssetMenu(fileName = "phrases", menuName = "Phrases", order = 0)]

public class LogoController : MonoBehaviour {

	public bool logoScreenBool = true;
	public GameObject panelNivel;
	public GameObject PanelPhrase;
	public GameObject PanelInfo;
	public GameObject logo;
	public Camera miCamara;
	Vector3 velocity = Vector3.zero;
	public Button miBoton;
	public Text textPhrases;

	private bool moveLevel = false;

	void Start () {
		panelNivel.SetActive(false);
		PanelPhrase.SetActive (false);
		textPhrases.text = GetRandomPhrase ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && logoScreenBool == true)
		{
			logo.SetActive (false);
			PanelPhrase.SetActive (true);
			StartCoroutine(goLevelWait(1));
		}
		move();
		//print(logoScreenBool);
		if (moveLevel)
			GoLevel ();
	}

	public void ButtonPress(){
		PanelPhrase.SetActive (false);
		logoScreenBool = false;
		miBoton.image.color = new Color (0f,0f,0f,0f);
	}

	public void ButtonPressLvl1(){
		//cargar escena del nivel 1
		logoScreenBool = true;
		moveLevel = true;
		StartCoroutine(goLevelWait(1));
	}
	public void ButtonPressLvl2(){
		//cargar escena del nivel 2
		if(GameManager.INSTANCE.GameLevel >= 2)
		{
			logoScreenBool = true;
			moveLevel = true;
			StartCoroutine(goLevelWait(2));
		}

	}
	public void ButtonPressLvl3(){
		//cargar escena del nivel 3
		if(GameManager.INSTANCE.GameLevel == 3)
		{
			logoScreenBool = true;
			moveLevel = true;
			StartCoroutine(goLevelWait(3));
		}

	}

	void move(){
		if(logoScreenBool == false){
			miCamara.transform.position = Vector3.SmoothDamp(miCamara.transform.position, new Vector3 (22.1f, 13.6f, -9.14f),ref velocity, 0.5f);
			PanelPhrase.SetActive (false);
			PanelInfo.SetActive (false);
			panelNivel.SetActive(true);
		}
	}

	public List<string> phrases; 

	public string GetRandomPhrase()
	{
		return phrases[Random.Range(0, phrases.Count)]; 
	}

	public void GoLevel (){
		
		miCamara.transform.position = Vector3.SmoothDamp (miCamara.transform.position, new Vector3 (32f, 21f, -9.14f), ref velocity, 0.5f);

		PanelPhrase.SetActive (true);
		PanelInfo.SetActive (false);
		panelNivel.SetActive(false);
	}

	public IEnumerator goLevelWait(int level){
		yield return new WaitForSeconds (3);
		Debug.Log ("Holaaaaaaaaaaaa");
		if(logoScreenBool == true)
			SceneManager.LoadScene (level);
	}
		
}
