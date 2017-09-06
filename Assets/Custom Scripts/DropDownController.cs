using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class DropDownController : Photon.MonoBehaviour
{

    public Dropdown DropdownMenu;
    static GameObject[] Prefabs;
    static List<GameObject> NetworkedPrefabs;
    static List<string> DropDownOptions;
    int CurrentPhotonID;
    int TargetPhotonID;
    public Button[] GUIButtons;
    public Material OutlineMaterial;
    // Use this for initialization
    void Start()
    {
        Debug.Log("DropDown started");
        PopulateList();

    }

    void PopulateList()
    {
        Prefabs = Resources.LoadAll("", typeof(GameObject)).Cast<GameObject>().ToArray();
        NetworkedPrefabs = new List<GameObject>();
        DropDownOptions = new List<string>() { "Molecules"};

        for (int i = 0; i < Prefabs.Length; i++)
        {
            if(!Prefabs[i].name.Contains("Swipe"))
            DropDownOptions.Add(Prefabs[i].name.Replace(".prefab", ""));
        }

        DropdownMenu.AddOptions(DropDownOptions);

        for (int i = 0; i < DropDownOptions.Count; i++)
        {
            Debug.Log("Dropdown options: " + DropDownOptions[i]);
        }
    }

    public void OnOptionChanged(int index)
    {
        string ActiveMolecule = DropDownOptions[index];
        Debug.Log(index + " " + ActiveMolecule + " is currently active");

        for (int i = 0, j = 0; i < Prefabs.Length; i++)
        {
            if (Prefabs[i].name.Contains(ActiveMolecule) && DropdownMenu.enabled)
            {
                Debug.Log(DropdownMenu.enabled + " inst");
                NetworkedPrefabs.Add(PhotonNetwork.Instantiate(Prefabs[i].name, new Vector3(0, 0, 0), Quaternion.identity, 0));
                j++;
                if (j == 0)
                {
                    TargetPhotonID = NetworkedPrefabs[i].GetPhotonView().viewID;
                }
                else
                {
                    TargetPhotonID = NetworkedPrefabs[j - 1].GetPhotonView().viewID;
                }
            }
        }
        //Display Buttons ony for Complex Molecules such as EDTA
        if (ActiveMolecule.Equals("EDTA") || ActiveMolecule.Equals("L-Alanine")|| ActiveMolecule.Equals("L-Glyceraldehyde"))
        {
            print("Display function calling");
            DisplayButtons(ActiveMolecule);
        }

        //Destroying the previous GO
        for (int i = 0; i < NetworkedPrefabs.Count; i++)
        {
            if (!(NetworkedPrefabs[i].name.Contains(ActiveMolecule)))
            {
                CurrentPhotonID = NetworkedPrefabs[i].GetPhotonView().viewID;
                PhotonNetwork.Destroy(NetworkedPrefabs[i]);
                NetworkedPrefabs.RemoveAt(i);
                NetworkedPrefabs.Sort();
            }
            if (!(ActiveMolecule.Equals("EDTA")) || !(ActiveMolecule.Equals("L-Alanine")) || !(ActiveMolecule.Equals("L-Glyceraldehyde")))
            {
                print("Calling hide buttons");
                HideDisplayButtons(ActiveMolecule);
            }
        }
        if(!ActiveMolecule.Equals("Molecules"))
        {
            PhotonView.Find(TargetPhotonID).photonView.RPC("OnDropDownOptionChanged", PhotonTargets.AllBufferedViaServer, CurrentPhotonID, TargetPhotonID);
            GUIButtons[5].gameObject.SetActive(true);
        }
        //Updated Networked Prefabs list
        for (int i = 0; i < NetworkedPrefabs.Count; i++)
        {
            Debug.Log("Updated list " + NetworkedPrefabs[i].name+" at "+i);
        }
    }

    //Display GUI Buttons
    private void DisplayButtons(string SelectedMolecule)
    {
        if (SelectedMolecule.Equals("EDTA"))
        {
            for (int i = 0; i <=2; i++)
            {
                GUIButtons[i].gameObject.SetActive(true);
            }
        }

        if(SelectedMolecule.Equals("L-Alanine"))
        {
            GUIButtons[3].gameObject.SetActive(true);
        }

        if(SelectedMolecule.Equals("L-Glyceraldehyde"))
        {
            GUIButtons[4].gameObject.SetActive(true);
        }

    }

    //Hide Display Buttons
    private void HideDisplayButtons(string SelectedMolecule)
    {
        if(!SelectedMolecule.Equals("EDTA"))
        {
            print("hiding EDTA");
            for (int i = 0; i <= 2; i++)
            {
                GUIButtons[i].gameObject.SetActive(false);
            }
        }

        if (!SelectedMolecule.Equals("L-Alanine"))
        {
            print("hiding Alanine");
            GUIButtons[3].gameObject.SetActive(false);
        }

        if (!SelectedMolecule.Equals("L-Glyceraldehyde"))
        {
            GUIButtons[4].gameObject.SetActive(false);
        }


    }

    public void HighlightDoubleBonds()
    {
        //Fetch the selected GO
        for (int i = 0; i < NetworkedPrefabs.Count; i++)
        {
            if (NetworkedPrefabs[i].name.Contains("EDTA"))
            {
                Debug.Log("Match");
                NetworkedPrefabs[i].GetPhotonView().RPC("HighlightDoubleBond", PhotonTargets.AllBufferedViaServer, NetworkedPrefabs[i].GetPhotonView().viewID);
            }
        }
        
    }

    public void HighlightCentralAtom()
    {
        for (int i = 0; i < NetworkedPrefabs.Count; i++)
        {
            if (NetworkedPrefabs[i].name.Contains("EDTA"))
            {
                Debug.Log("Match");
                NetworkedPrefabs[i].GetPhotonView().RPC("HighlightCentralAtom", PhotonTargets.AllBufferedViaServer, NetworkedPrefabs[i].GetPhotonView().viewID);
            }
        }
    }

    public void HighlightNitrogenAtom()
    {
        for (int i = 0; i < NetworkedPrefabs.Count; i++)
        {
            if (NetworkedPrefabs[i].name.Contains("EDTA"))
            {
                Debug.Log("Match");
                NetworkedPrefabs[i].GetPhotonView().RPC("HighlightNitrogenAtom", PhotonTargets.AllBufferedViaServer, NetworkedPrefabs[i].GetPhotonView().viewID);
            }
        }
    }

    public void OnClick_CompareDAlanine()
    {
        for (int i = 0; i < NetworkedPrefabs.Count; i++)
        {
            if(NetworkedPrefabs[i].name.Contains("L-Alanine"))
            {
                NetworkedPrefabs[i].transform.GetChild(1).gameObject.GetPhotonView().RPC("ShowDAlanine", PhotonTargets.AllBufferedViaServer, NetworkedPrefabs[i].transform.GetChild(1).gameObject.GetPhotonView().viewID);
            }
        }
    }

    public void OnClick_CompareDGlyceraldehyde()
    {
        for (int i = 0; i < NetworkedPrefabs.Count; i++)
        {
            if (NetworkedPrefabs[i].name.Contains("L-Glyceraldehyde"))
            {
                NetworkedPrefabs[i].transform.GetChild(1).gameObject.GetPhotonView().RPC("ShowDGlyceraldehyde", PhotonTargets.AllBufferedViaServer, NetworkedPrefabs[i].transform.GetChild(1).gameObject.GetPhotonView().viewID);
            }
        }
    }



    private void OnDestroy()
    {
        DropdownMenu.ClearOptions();
    }
}
