using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

	public float rotFactor;
	Plane objPlane;
	public GameObject gObj;
	Vector3 startPos, endPos, offset;
	Quaternion target;
	Touch currentPos;

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

		if (Input.touchCount == 2 && Input.GetTouch (0).phase == TouchPhase.Began &&
		    Input.GetTouch (1).phase == TouchPhase.Began || Input.GetMouseButtonDown (0)) {
			currentPos = Input.GetTouch (0);
			print ("id:" + currentPos.fingerId);
			//Ray touchRay = GenerateRay ();
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			float rayDistance;
			if (objPlane.Raycast (ray, out rayDistance)) {
				startPos = ray.GetPoint (rayDistance);
			}
		} else if (Input.touchCount == 2 && Input.GetTouch (0).phase == TouchPhase.Moved &&
		         Input.GetTouch (1).phase == TouchPhase.Moved || Input.GetMouseButtonDown (0)) {

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			float rayDistance;
			if (objPlane.Raycast (ray, out rayDistance)) {
				endPos = ray.GetPoint (rayDistance);

				offset += startPos +currentPos.deltaPosition;
				target = Quaternion.Euler (0, 0, offset.x * rotFactor);
				print ("target: " + target);
				gObj.transform.rotation = Quaternion.Slerp (gObj.transform.rotation, target, Time.deltaTime * 5.0f);

			}

		} else if (Input.touchCount == 2 && Input.GetTouch (0).phase == TouchPhase.Ended &&
		        Input.GetTouch (1).phase == TouchPhase.Ended || Input.GetMouseButtonUp (0)) {
			print (offset + "  " + gObj.transform.gameObject.name);


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
