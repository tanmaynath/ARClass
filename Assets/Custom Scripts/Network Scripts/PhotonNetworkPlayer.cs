using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vuforia;
using UnityEngine.EventSystems;

public class PhotonNetworkPlayer : Photon.MonoBehaviour
{

    Vector3 TargetPosition;
    Quaternion TargetRotation;
    Vector3 TargetScale;
    float smoothFactor = 10f;
    GameObject GameObjectSelected = null;
    Vector3 startPos;
    Plane objPlane;

    // Use this for initialization
    void Start()
    {
        //If the player is not the master client then disable all the interactions
        if (!photonView.isMine)
        {
            Behaviour[] translation = GetComponentsInChildren<Lean.Touch.LeanTranslateSmooth>();
            Behaviour[] rotation = GetComponentsInChildren<Lean.Touch.LeanRotate>();
            Behaviour[] scale = GetComponentsInChildren<Lean.Touch.LeanScale>();

            foreach (Behaviour disableTranslation in translation)
            {
                disableTranslation.enabled = false;
            }

            foreach (Behaviour disableRotation in rotation)
            {
                disableRotation.enabled = false;
            }

            foreach (Behaviour disableScale in scale)
            {
                disableScale.enabled = false;
            }

            if (GetComponent<Lean.Touch.PhotonLeanFingerTap>())
            {
                GetComponent<Lean.Touch.PhotonLeanFingerTap>().enabled = false;
            } 
            
            if (GetComponent<Lean.Touch.PhotonLeanFingerTapHighlight>())
            {
                GetComponent<Lean.Touch.PhotonLeanFingerTapHighlight>().enabled = false;
            }

            if(GetComponent<Lean.Touch.LeanFingerRay>())
            {
                GetComponent<Lean.Touch.LeanFingerRay>().enabled = false;
            }

            GetComponent<PhotonNetworkPlayer>().enabled = false;

            StartCoroutine("UpdateData");
        }

    }

    //Smooth interpolation of the position, rotation and scale
    IEnumerator UpdateData()
    {
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * smoothFactor);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, Time.deltaTime*500);
            transform.localScale = Vector3.Lerp(transform.localScale, TargetScale, 0.25f);

            yield return null;
        }

    }

    // Serialize the transform of the master client on the non-master client devices 
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(transform.localScale);
        }
        else
        {
            TargetPosition = (Vector3)stream.ReceiveNext();
            TargetRotation = (Quaternion)stream.ReceiveNext();
            TargetScale = (Vector3)stream.ReceiveNext();
        }
    }

    //RPC method when a GO is toggled between it's various forms
    [PunRPC]
    public void OnObjectChanged(int currentID, int targetID)
    {
        Debug.Log("current: " + currentID);
        Debug.Log("RPC set to true:" + targetID);
        MeshRenderer[] currentObjectMesh, targetObjectMesh;
        currentObjectMesh = PhotonView.Find(currentID).gameObject.GetComponentsInChildren<MeshRenderer>();
        targetObjectMesh=PhotonView.Find(targetID).gameObject.GetComponentsInChildren<MeshRenderer>();
        
        foreach(MeshRenderer mesh in currentObjectMesh)
        {
            mesh.enabled = false;
        }
        foreach(MeshRenderer mesh in targetObjectMesh)
        {
            mesh.enabled = true;
        }
    }

    //RPC method when a GO is selected from the dropdown menu
    [PunRPC]
    public void OnDropDownOptionChanged(int currentID, int targetID)
    {

        Debug.Log("current: " + currentID);
        Debug.Log("RPC set to true:" + targetID);
        MeshRenderer[] meshes;
        if (currentID != 0)
        {
            meshes= PhotonView.Find(currentID).gameObject.GetComponentsInChildren<MeshRenderer>();
            //meshes[0].enabled = false;
            foreach (MeshRenderer mesh in meshes)
            {
                mesh.enabled = false;
            }
        }

        meshes= PhotonView.Find(targetID).transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
        //meshes[0].enabled = true;
        foreach(MeshRenderer mesh in meshes)
        {
            mesh.enabled = true;
        }

    }

   
    //RPC method to highlight the double bond of the molecule EDTA
    [PunRPC]
    public void HighlightDoubleBond(int currentID)
    {
        //Fetch the original material and color of the first child GO
        Material[] OriginalMaterial=PhotonView.Find(currentID).transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;
        Color[] OriginalColor = new Color[OriginalMaterial.Length];

        //Save the Original Color
        for (int i = 0; i < OriginalMaterial.Length; i++)
        {
            OriginalColor[i] = OriginalMaterial[i].color;
        }

        Shader outlineshade = Shader.Find("Custom/Outline");
        Shader StandardShader = Shader.Find("Standard");
        
        //Change the shader to Outline shader or vice versa
        if (OriginalMaterial[0].shader == outlineshade)
        {
            OriginalMaterial[0].shader = StandardShader;
        }
        else
        {
            OriginalMaterial[0].shader = outlineshade;
            OriginalMaterial[0].SetFloat("_Outline", 0.15f);
            OriginalMaterial[0].SetColor("_OutlineColor", Color.green);
        }

        if (OriginalMaterial[1].shader == outlineshade)
        {
            OriginalMaterial[1].shader = StandardShader;
        }
        else
        {
            OriginalMaterial[1].shader = outlineshade;
            OriginalMaterial[1].SetFloat("_Outline", 0.15f);
            OriginalMaterial[1].SetColor("_OutlineColor", Color.green);
        }

    }

    //RPC method to highlight the central atom of the molecule EDTA
    [PunRPC]
    public void HighlightCentralAtom(int currentID)
    {
        //Fetch the original material and color of the GO
        Material[] OriginalMaterial = PhotonView.Find(currentID).transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().materials;
        Color[] OriginalColor = new Color[OriginalMaterial.Length];

        //Save the Original Color
        for (int i = 0; i < OriginalMaterial.Length; i++)
        {
            OriginalColor[i] = OriginalMaterial[i].color;
        }

        Shader outlineshade = Shader.Find("Custom/Outline");
        Shader StandardShader = Shader.Find("Standard");
        
        //Change the shader to Outline shader or vice versa
        for (int i = 0; i < OriginalMaterial.Length; i++)
        {
            if (OriginalMaterial[i].name.Contains("Metal"))
            {
                Debug.Log("Metal found");
                if (OriginalMaterial[i].shader == outlineshade)
                {
                    OriginalMaterial[i].shader = StandardShader;
                }
                else
                {
                    OriginalMaterial[i].shader = outlineshade;
                    OriginalMaterial[i].SetFloat("_Outline", 0.15f);
                    OriginalMaterial[i].SetColor("_OutlineColor", Color.green);
                }
            }
        }

    }

    //RPC method to highlight the nitrogen atoms of the molecule EDTA
    [PunRPC]
    public void HighlightNitrogenAtom(int currentID)
    {
        //Fetch the original material and color of the first child GO
        Material[] OriginalMaterial = PhotonView.Find(currentID).transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().materials;
        Color[] OriginalColor = new Color[OriginalMaterial.Length];

        //Save the Original Color
        for (int i = 0; i < OriginalMaterial.Length; i++)
        {
            OriginalColor[i] = OriginalMaterial[i].color;
        }

        Shader outlineshade = Shader.Find("Custom/Outline");
        Shader StandardShader = Shader.Find("Standard");

        //Change the shader to Outline shader or vice versa
        for (int i = 0; i < OriginalMaterial.Length; i++)
        {
            if (OriginalMaterial[i].name.Contains("Nitrogen"))
            {
                Debug.Log("Nitrogen found");
                if (OriginalMaterial[i].shader == outlineshade)
                {
                    OriginalMaterial[i].shader = StandardShader;
                }
                else
                {
                    OriginalMaterial[i].shader = outlineshade;
                    OriginalMaterial[i].SetFloat("_Outline", 0.15f);
                    OriginalMaterial[i].SetColor("_OutlineColor", Color.green);
                }
            }
        }

    }

    //RPC method to highlight an atom when it is tapped on
    [PunRPC]
    public void HighlightSelectedObject(int currentID)
    {
        GameObject GameObjectSelected = PhotonView.Find(currentID).gameObject;
        Material[] materials = GameObjectSelected.GetComponentInChildren<MeshRenderer>().materials;
        Shader StandardShader = Shader.Find("Standard");
        Shader OutlineShader = Shader.Find("Custom/Outline");

        //Change the shader to Outline shader or vice versa
        foreach (Material m in materials)
        {
            if (m.shader == OutlineShader)
            {
                m.shader = StandardShader;
                Debug.Log("Changed to standard");

            }
            else
            {
                m.shader = OutlineShader;
                m.SetFloat("_Outline", 0.15f);
                m.SetColor("_OutlineColor", Color.green);
                Debug.Log("Changed to Outline");
            }
        }
    }

    //RPC method to display the D-Alanine molecule
    [PunRPC]
    public void ShowDAlanine(int currentID)
    {
        print("D-alanine ID is: " + currentID);
        GameObject DAlanine = PhotonView.Find(currentID).gameObject;
        
        //Toggle the D-Alanine molecule
        foreach(MeshRenderer meshes in DAlanine.GetComponentsInChildren<MeshRenderer>())
        {
            meshes.enabled = !meshes.enabled;
        }
    }
    
    //RPC method to display the D-Glyceraldehyde molecule
    [PunRPC]
    public void ShowDGlyceraldehyde(int currentID)
    {
        print("D-Glyceraldehyde ID is: " + currentID);
        GameObject DGlyceraldehyde = PhotonView.Find(currentID).gameObject;

        //Toggle the D-Glyceraldehyde molecule
        foreach(MeshRenderer meshes in DGlyceraldehyde.GetComponentsInChildren<MeshRenderer>())
        {
            meshes.enabled = !meshes.enabled;
        }
    }

}
