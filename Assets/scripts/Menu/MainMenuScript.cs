using Assets.scripts.DialogModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    #region Unity Editor
    public Button MapMenuButton;               // Start Game button in main menu
    public Button SettingsMenuButton;
    #endregion

    void Start () {

        MapMenuButton.GetComponent<Button>().onClick.AddListener(MapMenuButtonClick);
        SettingsMenuButton.GetComponent<Button>().onClick.AddListener(SettingsMenuButtonClick);
    }

    private void SettingsMenuButtonClick()
    {
        SceneManager.LoadScene("Settings");
    }

    private void MapMenuButtonClick()
    {
        SceneManager.LoadScene("LevelMap");
    }

    private void ExitMenuButtonClick()
    {
        // Quit is ignored in the editor
        // https://docs.unity3d.com/ScriptReference/Application.Quit.html for reference
        Application.Quit();
    }
}
