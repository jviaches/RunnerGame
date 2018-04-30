using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public int timeToSurvive=60; //in seconds
	private int timeLeft;
	private LevelManagerScript lmScript;
	public Text clockText;

	// Use this for initialization
	void Start () {
		lmScript = GameObject.Find("LevelManager").GetComponent<LevelManagerScript>();
		clockText.text = timeLeft+"";
	}

	public void StartTimer(){
		RestartTimer ();
		Run ();
	}
	private void Run(){
		if (timeLeft == 0) {
			lmScript.FinishLevel (false);
		} else {
			timeLeft=timeLeft-1;
			clockText.text = timeLeft + "s";
			Debug.Log (timeLeft+"");
			Invoke ("Run", 1);
		}
	}

	public void RestartTimer(){
		timeLeft = timeToSurvive;
	}
	// Update is called once per frame
	void FixedUpdate () {
		
	}
}
