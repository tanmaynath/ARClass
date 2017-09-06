using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

	public GameObject stick;
	public GameObject ball;
	GameObject spawn;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick_Stick()
	{
		spawn = (GameObject)Instantiate (stick, this.transform.position, Quaternion.identity);
	}

	public void OnClick_Ball()
	{
		spawn = (GameObject)Instantiate (ball, this.transform.position, Quaternion.identity);
	}
}
