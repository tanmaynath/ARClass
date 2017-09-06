using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vuforia;

public class OfflineSwipe : MonoBehaviour {

	public GameObject swipeManager;
	public GameObject trailPrefab;
	public GameObject undo;
	GameObject thisTrail;
	Vector3 startPos;
	Plane objPlane;
	GameObject[] objCLones;

	// Use this for initialization
	void Start () {
		objPlane = new Plane (Camera.main.transform.forward * 1, this.transform.position);
	}

	// Update is called once per frame
	void Update () {

		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}

        //If there are no line trails then hide the undo button
        if (GameObject.Find("Trails").transform.childCount == 0)
        {
            undo.SetActive(false);
        }
			
        if (GameObject.FindWithTag("ImageTarget").activeSelf)
        {
			
            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
			{
				foreach (Touch touched in Input.touches)
				{
					if (EventSystem.current.IsPointerOverGameObject(touched.fingerId))
					{
						return;
					}
				}
                thisTrail = (GameObject)Instantiate(trailPrefab, this.transform.position, Quaternion.identity);
				print ("trail instantiated");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float rayDistance;
                if (objPlane.Raycast(ray, out rayDistance))
                {
                    startPos = ray.GetPoint(rayDistance);
                }


            }
            else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch (0).position);
                float rayDistance;
                if (objPlane.Raycast(ray, out rayDistance))
                {
                    thisTrail.transform.position = ray.GetPoint(rayDistance);
                }
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
            {
                if (Vector3.Distance(thisTrail.transform.position, startPos) < 0.3)
                {
                    Destroy(thisTrail);
                }
                thisTrail.gameObject.tag = "Trail";
                thisTrail.transform.SetParent(GameObject.Find("Trails").transform);
                undo.SetActive(true);
            }
        }
    }



	void OnClick_Draw()
	{
		//Toggle the drawing mode on and off
			swipeManager.SetActive (!swipeManager.activeSelf);


			Button draw = GameObject.Find ("Btn_Draw").GetComponent<Button> ();
			//print ("clicked draw");
			GameObject[] objClone = GameObject.FindGameObjectsWithTag ("ImageTarget");
			print ("length: " + objClone.Length);
			//If drawing mode is on then highlight the button
			if (swipeManager.activeSelf) 
			{
				draw.GetComponent<UnityEngine.UI.Image> ().color = Color.yellow;
				foreach (GameObject gObj in objClone) 
				{
					gObj.GetComponent<PerspectiveTouch> ().enabled = false;
					print ("obj: " + gObj.name);
				}
			} 
			else 
			{
				draw.GetComponent<UnityEngine.UI.Image> ().color = Color.white;
				foreach (GameObject gObj in objClone) 
				{
					gObj.GetComponent<PerspectiveTouch> ().enabled = true;
				}
		}

	}
}
