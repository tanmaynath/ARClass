using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeTrail : MonoBehaviour {

	public GameObject swipeManager;
	public GameObject trailPrefab;
	public GameObject undo;
	GameObject thisTrail;
	Vector3 startPos;
	Plane objPlane;
	int trailCount = 0;
    public GameObject DropDown;

	// Use this for initialization
	void Start () {
	 	objPlane = new Plane (Camera.main.transform.forward * 1, this.transform.position);
	}

    // Update is called once per frame
    void Update () {

		if (EventSystem.current.IsPointerOverGameObject())
		{
			Debug.Log("Hit UI, Ignore Touch");
			return;
		}

        //If the pointer is over the GUI then do not draw anything

        //If there are no line trails then hide the undo button
        if (GameObject.Find("Trails").transform.childCount == 0)
        {
            undo.SetActive(false);
        }

		if (DropDown.activeSelf)
        {
			if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
            {
				foreach (Touch touched in Input.touches)
				{
					if (EventSystem.current.IsPointerOverGameObject(touched.fingerId))
					{
						Debug.Log("Hit UI, Ignore Touch");
						return;
					}
				}
				thisTrail = (GameObject)PhotonNetwork.Instantiate("Swipe", this.transform.position, Quaternion.identity, 0);
				//thisTrail = (GameObject)Instantiate(trailPrefab, this.transform.position, Quaternion.identity);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				//Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch (0).position);
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
				if (Vector3.Distance(thisTrail.transform.position, startPos) < 0.3 && thisTrail!=null)
                {
                    Destroy(thisTrail);
                }
                thisTrail.gameObject.tag = "Trail";
                thisTrail.transform.SetParent(GameObject.Find("Trails").transform);
                undo.SetActive(true);

            }
        }
        
    }

	public void OnClick_Draw()
	{
        //Toggle the drawing mode on and off
		swipeManager.SetActive (!swipeManager.activeSelf);

        Button draw = GameObject.Find("Btn_Draw").GetComponent<Button>();

        GameObject ObjClone = GameObject.FindGameObjectWithTag("ImageTarget");
       

        
        //If drawing mode is on then highlight the button
        if (swipeManager.activeSelf)
        {
            draw.GetComponent<Image>().color = Color.yellow;
            ObjClone.GetComponentInChildren<Lean.Touch.LeanTranslateSmooth>().enabled = false;
        }
        else
        {
            draw.GetComponent<Image>().color = Color.white;
            ObjClone.GetComponentInChildren<Lean.Touch.LeanTranslateSmooth>().enabled = true;
        }
	}
}
