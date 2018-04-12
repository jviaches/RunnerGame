using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public Button StartMenuBtnButton;               // Start Game button in main menu

    void Start () {

        Button btn = StartMenuBtnButton.GetComponent<Button>();
        btn.onClick.AddListener(StartMenuButtonClick);
    }

    private void StartMenuButtonClick()
    {
        SceneManager.LoadScene("developmentScene");
        Debug.Log("StartMenuButtonClick()");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
