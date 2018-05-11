using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlayerDetect : MonoBehaviour {
	private bool detected =false;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	public void Reset(){
		detected = false;
	}

	void OnCollisionEnter(Collision collision)
	{
		if(!detected){
			if (collision.gameObject.name == "Buggy") {
				Debug.Log ("player Detected" + transform.position);// or if(gameObject.CompareTag("YourWallTag"))
				detected=true;
			}
		}
	}
}
