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
    public Button StartMenuButton;               // Start Game button in main menu
    public Button SoundMenuButton;
    public Button ExitMenuButton;               // Start Game button in main menu

    public GameObject MainModalPanel;
    public GameObject SoundModalPanel;
    public Button OkButton;
    public Text SoundDialogTitle;
    #endregion

    private DialogManager dialogManager;

    void Start () {

        StartMenuButton.GetComponent<Button>().onClick.AddListener(StartMenuButtonClick);
        SoundMenuButton.GetComponent<Button>().onClick.AddListener(SoundMenuButtonClick);
        ExitMenuButton.GetComponent<Button>().onClick.AddListener(ExitMenuButtonClick);

        dialogManager = new DialogManager();

        Dictionary<Button, UnityAction> soundModalDictionary = new Dictionary<Button, UnityAction>();
        soundModalDictionary.Add(OkButton, setSound);

        SoundDialogTitle.text = "Sound Settings";
        ModalDialog successModalDialog = new ModalDialog(SoundDialogTitle, SoundModalPanel, soundModalDictionary, MainModalPanel);
        dialogManager.AddDialog(successModalDialog);
    }

    private void SoundMenuButtonClick()
    {
        dialogManager.ShowModalDialog(SoundModalPanel, MainModalPanel);
    }

    private void setSound()
    {
        dialogManager.CloseAllOpenedModalDialogs();
    }

    private void StartMenuButtonClick()
    {
        SceneManager.LoadScene("LevelScene");
        Debug.Log("StartMenuButtonClick()");
    }

    private void ExitMenuButtonClick()
    {
        // Quit is ignored in the editor
        // https://docs.unity3d.com/ScriptReference/Application.Quit.html for reference
        Application.Quit();
        Debug.Log("ExitMenuButtonClick()");
    }
}
