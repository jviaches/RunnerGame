using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public delegate void LevelFailedAction();
    public static event LevelFailedAction LevelFailed;

    public delegate void LevelWonAction();
    public static event LevelWonAction LevelWon;

    public delegate void RestartLevelAction();
    public static event RestartLevelAction RestartLevel;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FireLevelWonEvent()
    {
        LevelWon();
    }

    public void FireLevelFailedEvent()
    {
        LevelFailed();
    }

    public void FireRestartLevelEvent()
    {
        RestartLevel();
    }
}
