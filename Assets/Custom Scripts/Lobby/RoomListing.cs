﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour {

    [SerializeField]
    private Text _roomNameText;
    private Text RoomNameText
    {
        get { return _roomNameText; }
    }

    public string RoomName { get; private set; }

    public bool Updated { get; set; }

	// Use this for initialization
	void Start () {
        GameObject lobbyCanvasObj = MainCanvasManager.Instance.LobbyCanvas.gameObject;
        if (lobbyCanvasObj == null)
        {
            return;
        }
        LobbyCanvas lobbyCanvas = lobbyCanvasObj.GetComponent<LobbyCanvas>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => lobbyCanvas.OnClickJoinRoom(RoomNameText.text));
	}

    private void OnDestroy()
    {
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }

    public void SetRoomText(string text)
    {
        RoomName = text;
        RoomNameText.text = RoomName ;
    }
}
