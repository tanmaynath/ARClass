using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoTrail : MonoBehaviour {

	public GameObject trails;
	public GameObject btnDraw;


	public void OnClick_NetworkedUndo()
	{
        if (trails.transform.childCount > 0)
        {
            PhotonNetwork.Destroy(trails.transform.GetChild(trails.transform.childCount - 1).gameObject);
        }

		print ("in undo: " + trails.transform.childCount);
	}

    public void OnClick_Undo()
    {
		//btnDraw.SetActive (false);

        if (trails.transform.childCount > 0)
        {
            Destroy(trails.transform.GetChild(trails.transform.childCount - 1).gameObject);
        }

        print("in undo: " + trails.transform.childCount);
    }

}
