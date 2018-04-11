using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScenesManager resposibility is to do all UI and scenes logic.

public class ScenesManager
{
    #region Singleton
    private volatile static ScenesManager _instance;
    private static object threadSynchronizer = new object();

    private ScenesManager() { }

    public static ScenesManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (threadSynchronizer)
                {
                    if (_instance == null)
                    {
                        _instance = new ScenesManager();
                    }
                }
            }

            return _instance;
        }
    }
    #endregion

    public int CurrentLevel = 1;        // level currently played by player (not nesesserely highest lvl number)

    private int higestCompletedLevel = 1; // higest completed level


    public void LoadLevel(int level)
    {
        // check if user already completed this level
        if (level > higestCompletedLevel)
        {
            // show some error
            LoadMainMenu();
            return;
        }

        CurrentLevel = level;

        // Load level
    }

    public void LoadNextLevel()
    {
        /* update UI */

        CurrentLevel++;

        if (CurrentLevel > higestCompletedLevel)
            higestCompletedLevel++;

        //load scene whith next level
    }

    #region Menus
    public void LoadMainMenu()
    {

    }

    public void LoadSettingsMenu()
    {

    }

    public void LoadScoreMenu()
    {

    }

    public void ExitGame()
    {

    }
    #endregion
}

