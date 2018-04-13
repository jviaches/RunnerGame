using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    // Defaults
    private const float LEVEL_TIMER = 120f;         // Default time to complete level
    private const int ROADBLOCK_POOLSIZE = 20;      // Default value for amount of roadblocks on scene

    #region assign from editor

    public GameObject RoadBlock;                    // Surface on which moving player's object
    public GameObject PlayerMovingObject;           // Actual player's object

    public GameObject SuccessModelPanel;            // Modal Dialog when user successfully completed level
    public Text SuccessModalTitle;                  // Caption of modal dialog
    public Button SuccessModalOkButton;             // OK button in Success modal dialog
    public Button SuccessModalNextLvlButton;        // Next level button in Success modal dialog

    public int Level = 1;                           // higher level will affect on RoadBlock direction change frequency

    public float LevelTimer = LEVEL_TIMER;          // Time to complete level (if not fall from wall)
    public Text LevelTimeText;                      // Updatable Timer in UI (upper bar)

    #endregion

    private Vector3 lastRoadBlockPos = new Vector3(0f, 0f, 0f); // position of last built roadBlock
    private Queue<GameObject> RoadBlockPool;        // for reuse GameObjects (memory, preformance and etc)

    private bool levelInProgress = false;           // if user is currently playing on current level
    private movableEntity PlayerMovingObjectScript; // Script of PlayerMovingObject

    public void Start()
    {
        SuccessModalNextLvlButton.GetComponent<Button>().onClick.AddListener(loadNextLevelModalDialog);
        SuccessModalOkButton.GetComponent<Button>().onClick.AddListener(loadMainMenuModalDialog);

        PlayerMovingObjectScript = PlayerMovingObject.GetComponent<movableEntity>();

        RoadBlockPool = new Queue<GameObject>(ROADBLOCK_POOLSIZE);

        loadNextLevelModalDialog();
    }

    public void Update()
    {
        if (levelInProgress)
        {
            LevelTimer -= Time.deltaTime;
            LevelTimeText.text = "Time: " + LevelTimer.ToString();

            Debug.Log(LevelTimer);
        }

        checkLevelConditions();
    }

    private void startLevel()
    {
        //Level++;

        restartLevelTimer();

        restartRoadBlocks();

        restartPlayer();

        levelInProgress = true;
    }

    private void restartLevelTimer()
    {
        LevelTimer = LEVEL_TIMER;
        LevelTimeText.text = "Timer: " + LevelTimer.ToString();
    }

    private void restartRoadBlocks()
    {
        lastRoadBlockPos = new Vector3(0f, 0f, 0f);

        foreach (GameObject roadBlock in RoadBlockPool.ToArray())
            Destroy(roadBlock);

        RoadBlockPool.Clear();

        for (int i = 0; i < ROADBLOCK_POOLSIZE; i++)
        {
            GameObject _platform = Instantiate(RoadBlock) as GameObject;
            _platform.transform.position = lastRoadBlockPos + new Vector3(1f, 0f, 0f);
            lastRoadBlockPos = _platform.transform.position;

            RoadBlockPool.Enqueue(_platform);
        }

        InvokeRepeating("spawnPlatform", 2f, 0.2f);
    }

    private void spawnPlatform()
    {
        int random = UnityEngine.Random.Range(0, 2);
        GameObject _platform = RoadBlockPool.Dequeue();

        if (random == 0)
            _platform.transform.position = lastRoadBlockPos + new Vector3(1f, 0f, 0f); // set on X
        else
            _platform.transform.position = lastRoadBlockPos + new Vector3(0f, 0f, 1f); // set on Z

        lastRoadBlockPos = _platform.transform.position;
        RoadBlockPool.Enqueue(_platform);
    }

    private void restartPlayer()
    {
        PlayerMovingObject.transform.position = new Vector3(0f, 1f, 0f);
        PlayerMovingObjectScript.SetSpeed(5f);
    }

    private void checkLevelConditions()
    {
        //if (PlayerMovingObjectScript.detectFreeFall())
        //{
        //    LevelTimer = 0;
        //    //ShowModalFailLevelCompletedDialog();

        //    return;
        //}

        if (LevelTimer <= 0)
        {
            LevelTimer = 0;

            // avoid repetitive call to showModalSuccessLevelCompletedDialog()
            if (false == levelInProgress)
                return;

            showModalSuccessLevelCompletedDialog();
        }
    }

    #region Dialogs
    private void showModalSuccessLevelCompletedDialog()
    {
        levelInProgress = false;
        CancelInvoke("spawnPlatform");

        PlayerMovingObjectScript.SetSpeed(0);

        SuccessModelPanel.SetActive(true);
        SuccessModalTitle.text = "Level " + Level + " completed !";

        Level++;
        
    }

    private void loadNextLevelModalDialog()
    {
        //Time.timeScale = 1; // resume scene from pause

        SuccessModelPanel.SetActive(false);
        
        startLevel();
    }

    private void loadMainMenuModalDialog()
    {
        //Time.timeScale = 1; // resume scene from pause
        SceneManager.LoadScene(0);
    }
    #endregion
}