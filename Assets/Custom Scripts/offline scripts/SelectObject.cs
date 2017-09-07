using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour {

	Plane objPlane;
	GameObject gObj;
	// Use this for initialization
	void Start () {
		objPlane = new Plane (Camera.main.transform.forward * 1, this.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.touchCount > 1){
			if (Input.GetTouch (0).phase == TouchPhase.Began && Input.GetTouch (1).phase == TouchPhase.Began) {
				
				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (1).position);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
				
					gObj = hit.collider.transform.gameObject;
					gObj.GetComponentInChildren <RotateObject> ().enabled = true;

				}
			}
			else if (Input.GetTouch (0).phase == TouchPhase.Ended && Input.GetTouch (1).phase == TouchPhase.Ended) {
				print ("touch ended for:" + gObj.name);
				gObj.GetComponentInChildren<RotateObject> ().enabled = false;
			}

		}
	}
}
