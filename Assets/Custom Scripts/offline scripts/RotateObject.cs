using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

	public float rotFactor;
	Plane objPlane;
	public GameObject gObj;
	Vector3 startPos, endPos, offset, midPoint;
	Quaternion target;
	Touch finger1, finger2;

//	Ray GenerateRay() {
//		Vector3 touchPosFar = new Vector3 (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y, Camera.main.farClipPlane);
//		Vector3 touchPosNear = new Vector3 (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y, Camera.main.nearClipPlane);
//
//		Vector3 touchPosF = Camera.main.ScreenToWorldPoint (touchPosFar);
//		Vector3 touchPosN = Camera.main.ScreenToWorldPoint (touchPosNear);
//
//		Ray ray = new Ray (touchPosN, touchPosF - touchPosN);
//		return ray;
//	}
	// Use this for initialization
	void Start () {
		objPlane = new Plane (Camera.main.transform.forward * -1, gObj.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		//Input.GetTouch (0).phase == TouchPhase.Began &&
		if (Input.touchCount == 2 && Input.GetTouch (0).phase == TouchPhase.Began &&
			Input.GetTouch (1).phase == TouchPhase.Began) {
			print ("here!!!!!!!!");
			finger1 = Input.GetTouch (0);
			finger2 = Input.GetTouch (1);
			 
			startPos = finger2.position;
			print ("start pos: " + startPos);
		} else if (Input.touchCount == 2 && 
			Input.GetTouch (1).phase == TouchPhase.Moved ) {

			endPos = finger2.position;
			print ("end pos: " + endPos);
			offset = finger2.deltaPosition;
			print ("offset: " + offset.x);
			print ("target before: " + target);
			target = Quaternion.Euler (0, 0, offset.x * rotFactor);
			print ("target: " + target);
			gObj.transform.rotation = Quaternion.Slerp (gObj.transform.rotation, target, Time.deltaTime * 5.0f);
			
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

		} 
			else if (Input.touchCount == 2 && Input.GetTouch (0).phase == TouchPhase.Ended &&
			Input.GetTouch (1).phase == TouchPhase.Ended || Input.GetMouseButtonDown (1)) {
			//print (offset + "  " + gObj.transform.gameObject.name);
			offset = new Vector3(0, 0, 0);


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
