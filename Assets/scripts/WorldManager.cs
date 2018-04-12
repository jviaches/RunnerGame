using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    #region assign from editor

    public GameObject RoadBlock;            // Surface on which moving player's object
    public GameObject PlayerMovingObject;   // Actual player's object
    
    public int Level = 1;                   // higher level will affect on RoadBlock direction change frequency

    public GameObject coin;                 // Prefab representing coins
    public int CoinsScore = 0;              // How many coins picked up in whole levels
    public Text coinText;                   // Update coins amount on game screen

    public int LevelTimer = 120;            // Default time to complete level (if not fall from wall)
    public Text LevelTimeText;              // Updatable Timer in UI

    #endregion
    private Vector3 lastRoadBlockpos = new Vector3(0f, 0f, 0f);
    private Queue<GameObject> RoadBlockPool;       // for reuse GameObjects (memory, preformance and etc)
    
    private float timer;                            // [Countdown], limits time to pass current level  
    private bool playing = false;                   // if user is currently playing on current level
    private movableEntity PlayerMovingObjectScript; // Script of PlayerMovingObject

    public void Start()
    {
        RoadBlockPool = new Queue<GameObject>(20);
        coinText.text = "Coins: " + CoinsScore.ToString();
        LevelTimeText.text = "Timer: " + LevelTimer.ToString();

        PlayerMovingObjectScript = PlayerMovingObject.GetComponent<movableEntity>();
        PlayerMovingObjectScript.CollectedCoins += PlayerMovingObjectScript_CollectedCoins;

        for (int i = 0; i < 20; i++)
        {
            GameObject _platform = Instantiate(RoadBlock) as GameObject;
            _platform.transform.position = lastRoadBlockpos + new Vector3(1f, 0f, 0f);
            lastRoadBlockpos = _platform.transform.position;

            RoadBlockPool.Enqueue(_platform);
        }

        InvokeRepeating("SpawnPlatform", 2f, 0.2f);

        Level = ScenesManager.Instance.CurrentLevel;
        playing = true;
    }

    private void PlayerMovingObjectScript_CollectedCoins(object sender, System.EventArgs e)
    {
        CoinsScore++;
    }

    void SpawnPlatform()
    {
        GameObject _platform;
        GameObject _coin;

        int random = Random.Range(0, 2);
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

        if (Random.value > 0.5) //%5
        {
            _coin = Instantiate(coin) as GameObject;
            _coin.transform.position = lastRoadBlockpos + new Vector3(0f, 1f, 0f);
        }

        RoadBlockPool.Enqueue(_platform);
    }

    public void Update()
    {
        coinText.text = "Coins: " + CoinsScore.ToString();
        // randomize next roadblock for X or Z axis (depending on level)
        // & reuse roadblock from RoadBlockPool

        if (playing)
        {
            // run countdown

            GameOverCheck();
            LevelCompleteCheck();
        }
    }


    // checking if user failed to pass level
    private void GameOverCheck()
    {
        if (timer > 0f || PlayerMovingObjectScript.detectFreeFall())
        {
            playing = !playing;     // stop timer

            /* update UI
             ...
             move user to the the score screen  */
        }
    }

    // checking if user Succeded to pass level
    private void LevelCompleteCheck()
    {
        if (timer == 0f && !PlayerMovingObjectScript.detectFreeFall())
        {
            playing = !playing;     // stop timer

            ScenesManager.Instance.LoadNextLevel();
        }
    }
}

