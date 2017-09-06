using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineSceneNavigation : MonoBehaviour {

	public void OnClick_HomeButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClick_ExitButton()
    {
        Application.Quit();
    }
}
