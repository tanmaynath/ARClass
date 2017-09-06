using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentRoomCanvas : MonoBehaviour {

    public void OnClickStartSync()
    {
        if(!PhotonNetwork.isMasterClient)
        {
            return; 
        }
       PhotonNetwork.LoadLevel(1);
    }
}
