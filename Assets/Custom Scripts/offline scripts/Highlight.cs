using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {


	Material[] materials;
	// Use this for initialization
	void Start () {
		print ("highlight started.");
	}


	Ray GenerateTouchRay()
	{
		Vector3 touchPosNear = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
		Vector3 touchPosFar = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);

		Vector3 touchPosN = Camera.main.ScreenToWorldPoint (touchPosNear);
		Vector3 touchPosF = Camera.main.ScreenToWorldPoint (touchPosFar);

		Ray ray = new Ray (touchPosN, touchPosF - touchPosN);

		return ray;
	}
	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1) 
		{

			print ("in update function.");
			Ray touchRay = GenerateTouchRay ();
			Shader StandardShader = Shader.Find("Standard");
			Shader OutlineShader = Shader.Find("Custom/Outline");

			RaycastHit hit;
			if (Physics.Raycast (touchRay.origin, touchRay.direction, out hit)) {
				GameObject objSelected = hit.transform.gameObject;
				print ("selected: " + objSelected.name);
				if (Input.GetTouch (0).phase == TouchPhase.Began) {
					materials = objSelected.GetComponent <MeshRenderer> ().materials;
					//objSelected.GetComponent <LineRenderer> ().enabled = true;
					objSelected.transform.GetChild (0).gameObject.SetActive (true);
				}
				if (Input.GetTouch (0).phase == TouchPhase.Ended) {
					foreach (Material m in materials) {
						if (m.shader == OutlineShader) {
							m.shader = StandardShader;
						}
						else 
						{
							m.shader = OutlineShader;
							m.SetFloat ("_Outline", 0.15f);
							m.SetColor ("_OutlineColor", Color.green);
							Debug.Log ("Changed to Outline");
						}
					}
				}
			}
		}
	}
}
