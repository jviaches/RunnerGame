using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class WorldManager : MonoBehaviour
{
    #region assign from editor

    public GameObject RoadBlock;            // Surface on which moving player's object
    public GameObject PlayerMovingObject;   // Actual player's object

    public GameObject ModelPanel;           // Modal Dialog
    public Text ModalText;                  // Caption of modal dialog
    public Button ModalOkButton;            // OK button in modal dialog

    public int Level = 0;                   // higher level will affect on RoadBlock direction change frequency

    public GameObject coin;                 // Prefab representing coins
    public int CoinsScore = 0;              // How many coins picked up in whole levels
    public Text coinText;                   // Update coins amount on game screen

    public float LevelTimer = 4;          // Default time to complete level (if not fall from wall)
    public Text LevelTimeText;              // Updatable Timer in UI

    #endregion
    private Vector3 lastRoadBlockpos = new Vector3(0f, 0f, 0f);
    private Queue<GameObject> RoadBlockPool;        // for reuse GameObjects (memory, preformance and etc)

    private bool playing = false;                   // if user is currently playing on current level
    private movableEntity PlayerMovingObjectScript; // Script of PlayerMovingObject

    public void Start()
    {
        ModalOkButton.GetComponent<Button>().onClick.AddListener(HideModalDialog);

        PlayerMovingObjectScript = PlayerMovingObject.GetComponent<movableEntity>();
        PlayerMovingObjectScript.CollectedCoins += PlayerMovingObjectScript_CollectedCoins;

        RoadBlockPool = new Queue<GameObject>(20);

        HideModalDialog();
    }

    private void StartLevel()
    {
        foreach (GameObject roadBlock in RoadBlockPool.ToArray())
            Destroy(roadBlock);

        Level++;
        
        coinText.text = "Coins: " + CoinsScore.ToString();
        LevelTimeText.text = "Timer: " + LevelTimer.ToString();

        lastRoadBlockpos = new Vector3(0f, 0f, 0f);
        PlayerMovingObject.transform.position = new Vector3(0f, 1f, 0f);

        RoadBlockPool.Clear();

        for (int i = 0; i < 20; i++)
        {
            GameObject _platform = Instantiate(RoadBlock) as GameObject;
            _platform.transform.position = lastRoadBlockpos + new Vector3(1f, 0f, 0f);
            lastRoadBlockpos = _platform.transform.position;

            RoadBlockPool.Enqueue(_platform);
        }

        InvokeRepeating("SpawnPlatform", 2f, 0.2f);

        PlayerMovingObjectScript.SetSpeed(5f);

        playing = true;
        LevelTimer = 4;
    }

    private void PlayerMovingObjectScript_CollectedCoins(object sender, System.EventArgs e)
    {
        CoinsScore++;
    }

    void SpawnPlatform()
    {
        GameObject _platform;
        GameObject _coin;

        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            _platform = RoadBlockPool.Dequeue();
            _platform.transform.position = lastRoadBlockpos + new Vector3(1f, 0f, 0f); // set on X
            lastRoadBlockpos = _platform.transform.position;
        }
        else
        {
            _platform = RoadBlockPool.Dequeue();
            _platform.transform.position = lastRoadBlockpos + new Vector3(0f, 0f, 1f); // set on Z
            lastRoadBlockpos = _platform.transform.position;
        }

        if (UnityEngine.Random.value > 0.85) //15%
        {
            _coin = Instantiate(coin) as GameObject;
            _coin.transform.position = lastRoadBlockpos + new Vector3(0f, 1f, 0f);
        }

        RoadBlockPool.Enqueue(_platform);
    }

    public void Update()
    {
        if (playing)
        {
            coinText.text = "Coins: " + CoinsScore.ToString();
            LevelTimeText.text = "Time: " + LevelTimer;
        }
        else
        {
            LevelTimer = 0;
            ShowModalLevelCompletedDialog();
        }

        LevelTimer -= Time.deltaTime;
        if (LevelTimer <= 0)
        {
            LevelTimer = 0;
            PlayerMovingObject.transform.position = PlayerMovingObject.transform.position;
            playing = false;
        }     
    }

    private void HideModalDialog()
    {
        ModelPanel.SetActive(false);
        StartLevel();
    }

    private void ShowModalDialog()
    {
        ModelPanel.SetActive(true);
    }

    private void ShowModalLevelCompletedDialog()
    {
        CancelInvoke("SpawnPlatform");
        PlayerMovingObjectScript.SetSpeed(0);
        playing = false;

        ModelPanel.SetActive(true);
        ModalText.text = "Level " + Level + " completed !";
    }
}

