using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyNetwork : MonoBehaviour {

    [SerializeField]
    private GameObject _ddol;
    public GameObject DDOLGameObject
    {
        get { return _ddol; }
    }
    
    
    // Use this for initialization
	void Start () {
        print("Connecting to server");
        //PhotonNetwork.ConnectUsingSettings("1.0.0");
	}
	
    private void OnConnectedToMaster()
    {
        print("Connected to master");
        PhotonNetwork.automaticallySyncScene = true;
        //PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;
        PhotonNetwork.playerName = LoginCanvas.Instance.PlayerNameInput.text;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        print("Joined Lobby");

        if (!PhotonNetwork.inRoom)
        {
            MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
        }
    }

    private void Awake()
    {
        if(GameObject.Find("DDOL(Clone)")==null)
        {
            Instantiate(DDOLGameObject);
        }
    }

}
