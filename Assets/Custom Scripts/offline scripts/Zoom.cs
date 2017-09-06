using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour {

	private GameObject selectedObject = null;
	private Vector3 initialScale;
	public float zoomSpeed =0.5f;
	float firstTouchPos;
	float currentTouchPos;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

		if (Input.touchCount == 3) {
			
			Touch finger1 = Input.GetTouch (0);
			Touch finger2 = Input.GetTouch (1);

			if (finger1.phase == TouchPhase.Began && finger2.phase == TouchPhase.Began) {
				Touch touch = Input.touches [0];
				Ray ray = Camera.main.ScreenPointToRay (touch.position);

				RaycastHit hit;

				if (Physics.Raycast (ray, out hit, 1000f)) {
			
					selectedObject = hit.transform.parent.gameObject;
				}

				initialScale = selectedObject.transform.localScale;

				Debug.Log ("starting size: " + initialScale);
				firstTouchPos = (finger1.position - finger2.position).magnitude;
			}
		
		
			else if (finger1.phase == TouchPhase.Moved && finger2.phase == TouchPhase.Moved) {
			

			currentTouchPos = (finger1.position - finger2.position).magnitude;

			float scaleFactor = currentTouchPos/firstTouchPos;
			Debug.Log ("Scale factor: " + scaleFactor);
			selectedObject.transform.localScale = initialScale * scaleFactor;
			}
			else if (finger1.phase == TouchPhase.Ended && finger2.phase == TouchPhase.Ended) {

				selectedObject = null;
			}
		}
	}
}
