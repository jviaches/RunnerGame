using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackWallCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	void OnParticleCollision(GameObject other) {
		if(other.name=="PlayerBody")
		Debug.Log ("Game Over");
	}
}
