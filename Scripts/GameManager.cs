using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager INSTANCE;

	private int _gameLevel = 1;

    private static bool created = false;

    void Awake()
    {
        if (!created)
        {
			INSTANCE = this;
            DontDestroyOnLoad(this.gameObject);
            created = true;
            Debug.Log("Awake: " + this.gameObject);
        }
    }

    public int GameLevel
    {
        set { _gameLevel = value; }
        get { return _gameLevel; }
    }

}
