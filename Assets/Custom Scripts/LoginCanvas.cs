using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginCanvas : MonoBehaviour {

    public static LoginCanvas Instance;

    [SerializeField]
    private Text _playerNameInput;

    public Text PlayerNameInput
    {
        get { return _playerNameInput; }
    }
    
    private void Awake()
    {
        Instance = this;
    }

    public void OnClick_LoginButton()
    {
        PhotonNetwork.playerName = PlayerNameInput.text;
        PhotonNetwork.ConnectUsingSettings("1.0.0");
    }

    public void OnClick_OfflineButton()
    {
        SceneManager.LoadScene(2);
    }

    public void OnClick_ExitButton()
    {
        Application.Quit();
    }
}
