using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontWallAnimation : MonoBehaviour {

	public ParticleSystem debreeSystem;
	public ParticleSystem smokeSystem;

    private void OnEnable()
    {
        EventManager.LevelFailed += StopAnimation;
        EventManager.LevelWon += StopAnimation;
        EventManager.RestartLevel += StartAnimation;
    }

    private void OnDisable()
    {
        EventManager.LevelFailed -= StopAnimation;
        EventManager.LevelWon -= StopAnimation;
        EventManager.RestartLevel -= StartAnimation;

    }
    void Start () {
		
	}
    private void StartAnimation()
    {
        debreeSystem.Play();
        smokeSystem.Play();
    }
    private void StopAnimation(){
		debreeSystem.Stop ();
		smokeSystem.Stop ();
	}
}
