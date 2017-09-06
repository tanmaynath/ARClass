using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCanvas : MonoBehaviour {

    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    private RoomLayoutGroup RoomLayoutGroup
    {
        get { return _roomLayoutGroup; }
    }

    [SerializeField]
    private GameObject _waitingText;
    private GameObject WaitingText
    {
        get { return _waitingText; }
    }

    public void OnClickJoinRoom(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName))
        {
            print("Joined the room");
            if (!PhotonNetwork.isMasterClient && GameObject.Find("StartClass")!=null)
            {

                GameObject StartClassObj = GameObject.Find("StartClass");
                StartClassObj.SetActive(false);
                print("Button hide");
                WaitingText.SetActive(true);
            }

        }
        else print("Joined Room failed");
    }

    public void OnClick_BackButton()
    {
        MainCanvasManager.Instance.LoginCanvas.transform.SetAsLastSibling();
        PhotonNetwork.Disconnect();
    }
}
