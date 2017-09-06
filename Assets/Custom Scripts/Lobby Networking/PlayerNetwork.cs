using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerNetwork : Photon.MonoBehaviour {

    public static PlayerNetwork Instance;
    private PhotonView PhotonView;

    private void Awake()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();    
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode )
    {
        if(scene.name == "NetworkScene")
        {
            PhotonPlayer[] photonPlayer = PhotonNetwork.playerList;
            if (PhotonNetwork.isMasterClient && photonPlayer.Length>1)
                MasterLoadedGame();
            else
                NonMasterLoadedGame();
        }
    }

    private void MasterLoadedGame()
    {
        PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
    }

    private void NonMasterLoadedGame()
    {

    }


    [PunRPC]

    private void RPC_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel(1);
    }


}
