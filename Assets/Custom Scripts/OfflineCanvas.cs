using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OfflineCanvas : MonoBehaviour {

    GameObject[] MoleculesInScene; 
    public Button CompareDGlycerladehyde;
	// Use this for initialization
	void Start () {
		MoleculesInScene = GameObject.FindGameObjectsWithTag("ImageTarget"); 
    }
	
	// Update is called once per frame
	void Update () {

        //print(MoleculesInScene.Length);

        for (int i = 0; i < MoleculesInScene.Length; i++)
        {
        
            if(MoleculesInScene[i].name.Equals("L-Glyceraldehyde"))
            {
                //If L-Glyceraldehyde is currently active on the screen then show the button else hide
                MeshRenderer[] meshes = MoleculesInScene[i].GetComponentsInChildren<MeshRenderer>();
                if (meshes[i].enabled)
                {
                    CompareDGlycerladehyde.gameObject.SetActive(true);
                    //print("Showing the button");
                }
                else
                {
                    CompareDGlycerladehyde.gameObject.SetActive(false);
                }
            }
            
        }

	}

    public void OnClick_ShowDGlycerladehyde()
    {
        GameObject GlyceraldehydeImageTarget = GameObject.Find("Glyceraldehyde ImageTarget");
        //Toggle the second child GO- D-Glyceraldehyde
        GlyceraldehydeImageTarget.transform.GetChild(1).gameObject.SetActive(!GlyceraldehydeImageTarget.transform.GetChild(1).gameObject.activeSelf);
    }

	public void OnClick_BackButton()
	{
		SceneManager.LoadScene(0);
	}
	public void OnClick_ExitButton()
	{
		Application.Quit();
	}
}
