using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public Button StartMenuBtnButton;               // Start Game button in main menu
    public Button ExitMenuBtnButton;               // Start Game button in main menu

    void Start () {

        StartMenuBtnButton.GetComponent<Button>().onClick.AddListener(StartMenuButtonClick);
        ExitMenuBtnButton.GetComponent<Button>().onClick.AddListener(ExitMenuButtonClick);
    }

    private void StartMenuButtonClick()
    {
        SceneManager.LoadScene("developmentScene");
        Debug.Log("StartMenuButtonClick()");
    }

    private void ExitMenuButtonClick()
    {
        // Quit is ignored in the editor
        // https://docs.unity3d.com/ScriptReference/Application.Quit.html for reference
        Application.Quit();
        Debug.Log("ExitMenuButtonClick()");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
