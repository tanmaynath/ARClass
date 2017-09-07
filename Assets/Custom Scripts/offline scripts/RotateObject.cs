﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

	public float rotFactor;
	Plane objPlane;
	public GameObject gObj;
	Vector3 startPos, endPos, midPoint;
	Quaternion target;
	Touch finger1, finger2;
	float offset;

	// Use this for initialization
	void Start () {
		objPlane = new Plane (Camera.main.transform.forward * -1, gObj.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		//Input.GetTouch (0).phase == TouchPhase.Began &&
		if (Input.touchCount == 2) {
			
			if (Input.GetTouch (0).phase == TouchPhase.Began && Input.GetTouch (1).phase == TouchPhase.Began) {
				print ("here!!!!!!!!");
				finger1 = Input.GetTouch (0);
				finger2 = Input.GetTouch (1);

				startPos = finger2.position;
				print ("start pos: " + startPos);
			} 
			else if (Input.GetTouch (1).phase == TouchPhase.Moved) {
				finger2 = Input.GetTouch (1);
				endPos = finger2.position;
				print ("end pos: " + endPos);
				offset = finger2.deltaPosition.x * Time.deltaTime;
				if(endPos.x > startPos.x){ 
					
					print ("offset: " + offset);
					print ("target before: " + target);
					target = Quaternion.Euler (0, 0, offset * rotFactor);
					print ("target: " + target);
					gObj.transform.rotation = Quaternion.Slerp (gObj.transform.rotation, target, Time.deltaTime * 5.0f);

				}
//			Ray ray = Camera.main.ScreenPointToRay (finger2.position);
//			float rayDistance;
//			if (objPlane.Raycast (ray, out rayDistance)) {
//				print("moved");
//				endPos = ray.GetPoint (rayDistance);
//
//				offset = startPos - endPos;
//				print ("offset: " + offset.x);
//				print ("target before: " + target);
//				target *= Quaternion.Euler (0, 0, offset.x * rotFactor);
//				print ("target: " + target);
//				gObj.transform.rotation = Quaternion.Slerp (gObj.transform.rotation, target, Time.deltaTime * 5.0f);
//
//			}

			} else if (Input.touchCount == 2 && Input.GetTouch (0).phase == TouchPhase.Ended &&
			         Input.GetTouch (1).phase == TouchPhase.Ended || Input.GetMouseButtonDown (1)) {
				//print (offset + "  " + gObj.transform.gameObject.name);
				offset = 0.0f;


			}


//			target = Quaternion.Euler (0, 0, mO.x * rotFactor);
//				
////				if (Mathf.Abs(mO.x) < 3.0f) {
////
////					target = Quaternion.Euler (mO.z * rotFactor, 0, 0);
////				}
//				print ("target: " + target);
//				gObj.transform.rotation = Quaternion.Slerp(gObj.transform.rotation, target, Time.deltaTime * 2.0f);
//				print ("rotation: " + gObj.transform.rotation);
//

			


		}
	}
}
