using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour {

    #region Unity Editor
    public Button MainMenuButton;

    public Button Lvl1MenuButton;
    public Button Lvl2MenuButton;
    public Button Lvl3MenuButton;
    public Button Lvl4MenuButton;
    public Button Lvl5MenuButton;
    public Button Lvl6MenuButton;
    #endregion

    void Start()
    {
        MainMenuButton.GetComponent<Button>().onClick.AddListener(MainMenuButtonClick);

        Lvl1MenuButton.GetComponent<Button>().onClick.AddListener(Lvl1MenuButtonClick);
        Lvl2MenuButton.GetComponent<Button>().onClick.AddListener(Lvl2MenuButtonClick);
        Lvl3MenuButton.GetComponent<Button>().onClick.AddListener(Lvl3MenuButtonClick);
        Lvl4MenuButton.GetComponent<Button>().onClick.AddListener(Lvl4MenuButtonClick);
        Lvl5MenuButton.GetComponent<Button>().onClick.AddListener(Lvl5MenuButtonClick);
        Lvl6MenuButton.GetComponent<Button>().onClick.AddListener(Lvl6MenuButtonClick);
    }

    private void MainMenuButtonClick()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void Lvl1MenuButtonClick()
    {
        SceneManager.LoadScene("Level1");
    }

    private void Lvl2MenuButtonClick()
    {
        SceneManager.LoadScene("Level2");
    }

    private void Lvl3MenuButtonClick()
    {
        SceneManager.LoadScene("Level3");
    }

    private void Lvl4MenuButtonClick()
    {
        SceneManager.LoadScene("Level4");
    }

    private void Lvl5MenuButtonClick()
    {
        SceneManager.LoadScene("Level5");
    }

    private void Lvl6MenuButtonClick()
    {
        SceneManager.LoadScene("Level1");
    }
}
