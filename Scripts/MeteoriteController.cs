using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeteoriteController : MonoBehaviour {

	public float loopTime = 10f;

	public Vector3[] waypoints = new Vector3[4];
	private Transform meteorite;

	// Use this for initialization
	void Start () {
		transform.parent.GetChild(1);
		Transform path = transform.parent.GetChild(1);
		int i = 0;
		foreach (Transform child in path)
		{
			waypoints[i] = child.position;
			i++;			
		}

		MeteoriteCicle();
	}
	
	void MeteoriteCicle()
	{
		transform.DOPath(waypoints, loopTime).SetEase(Ease.Linear);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "ExitMeteorite")
		{
			MeteoriteCicle();
		}
		
	}
}
