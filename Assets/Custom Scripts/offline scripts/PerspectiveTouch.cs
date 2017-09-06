using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PerspectiveTouch : MonoBehaviour {

	// Use this for initialization
	GameObject gObj, gObj_rotate;
	Plane objPlane;
	Vector3 mO;
	Touch finger1, finger2;
	Transform newRotation;

//
	Ray GenerateTouchRay(int index){

			Vector3 touchPosFar = new Vector3 (Input.GetTouch (index).position.x, Input.GetTouch (index).position.y, Camera.main.farClipPlane);
			Vector3 touchPosNear = new Vector3 (Input.GetTouch (index).position.x, Input.GetTouch (index).position.y, Camera.main.nearClipPlane);

			Vector3 touchPosF = Camera.main.ScreenToWorldPoint (touchPosFar);
			Vector3 touchPosN = Camera.main.ScreenToWorldPoint (touchPosNear);

			Ray ray = new Ray (touchPosN, touchPosF - touchPosN);

			return ray;
	
	}

	void Start () {
		Debug.Log ("scipt started");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1) {
			if (Input.GetTouch (0).phase == TouchPhase.Began) {

				Ray touchRay = GenerateTouchRay (0);
				RaycastHit hit;

				if (Physics.Raycast (touchRay.origin, touchRay.direction, out hit)) {
					gObj = hit.transform.parent.gameObject;
					print ("in touch: " + gObj.name);
					objPlane = new Plane (Camera.main.transform.forward * -1, gObj.transform.position);

					Ray mRay = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
					float rayDistance;
					objPlane.Raycast (mRay, out rayDistance);
					mO = gObj.transform.position - mRay.GetPoint (rayDistance);
				}
			} else if (Input.GetTouch (0).phase == TouchPhase.Moved) {
				
				Ray mRay = Camera.main.ScreenPointToRay ((Input.GetTouch (0).position));
				float rayDistance;
				if (objPlane.Raycast (mRay, out rayDistance)) {
					gObj.transform.position = Vector3.Lerp (mRay.GetPoint (rayDistance), mRay.GetPoint (rayDistance) + mO, 1.0f);
				}


			
			} else if (Input.GetTouch (0).phase == TouchPhase.Ended) 
			{
				gObj = null;			
			}

		}
			
	}
}
