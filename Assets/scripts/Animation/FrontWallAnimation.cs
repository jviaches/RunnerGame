using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontWallAnimation : MonoBehaviour {

	public ParticleSystem debreeSystem;
	public ParticleSystem smokeSystem;

	// Use this for initialization
	void Start () {
		
	}
	
	public void StopAnimation(){
		debreeSystem.Stop ();
		smokeSystem.Stop ();
	}
}
