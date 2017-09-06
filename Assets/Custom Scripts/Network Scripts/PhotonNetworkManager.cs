using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;
using System.Collections;

public class PhotonNetworkManager : MonoBehaviour
{
    public GameObject[] DropDownObjects;
    CustomDefaultTrackableEventHandler eventHandle;

    private void Awake()
    {
        eventHandle = GameObject.Find("MainImageTarget").GetComponent<CustomDefaultTrackableEventHandler>();
    }

    private void Update()
    {
        //If the current player is the master client and the image tracker is present
        if (PhotonNetwork.isMasterClient)
        {
            if (eventHandle.IsExist())
            {
                DropDownObjects[0].SetActive(true);
                DropDownObjects[1].SetActive(true);
            }
            else
            {
                DropDownObjects[0].SetActive(false);
                DropDownObjects[1].SetActive(false);
            }
        }
        else
        {
            return;
        }
    }

    private void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        print("On master client switched called");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }

    private void PlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        if (!photonPlayer.IsMasterClient)
        {
            print("client left room");
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(0);
        }
    }

    public void OnClick_LeaveClass()
    {
        print("leave class");
        if (PhotonNetwork.isMasterClient)
        {
            OnMasterClientSwitched(PhotonNetwork.player);
        }
        else
        {
            PlayerLeftRoom(PhotonNetwork.player);
        }
    }
}