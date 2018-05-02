using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackWallCollision : MonoBehaviour {

	public ParticleSystem fallingStones;
	public LevelManagerScript lmScript;

	// Use this for initialization
	void Start () {
		lmScript = GameObject.Find("LevelManager").GetComponent<LevelManagerScript>();
	}
	void OnParticleCollision(GameObject other) {
		if (other.name == "Buggy") {
			//Debug.Log ("Game Over");
			fallingStones.Stop ();
			lmScript.FinishLevel (true);
		}
	}
}
