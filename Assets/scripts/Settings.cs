using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    #region Unity Editor
    public Button OkMenuButton;
    public Button MusicMenuButton;
    public Button EffectsMenuButton;
    #endregion

    void Start()
    {
        OkMenuButton.GetComponent<Button>().onClick.AddListener(OkMenuButtonClick);
        MusicMenuButton.GetComponent<Button>().onClick.AddListener(MusicMenuButtonClick);
        EffectsMenuButton.GetComponent<Button>().onClick.AddListener(EffectsMenuButtonClick);
    }

    private void OkMenuButtonClick()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void MusicMenuButtonClick()
    {
        if (WorldManager.Instance.MusicOff)
        {
            // set visual on button

            // music off in this scene
        }
    }

    private void EffectsMenuButtonClick()
    {
        Debug.Log("Effects still not wired");
    }
}
