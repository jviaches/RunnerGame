using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlayerDetect : MonoBehaviour {
	private bool detected =false;
    private LevelManagerScript script_LvelManager;

	// Use this for initialization
	void Start () {
        script_LvelManager = GameObject.Find("LevelManager").GetComponent<LevelManagerScript>();
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
                ((EventManager)script_LvelManager.GetScript("EventManager")).FirePlayerLocationChange(transform.position);
				//Debug.Log ("player Detected" + transform.position);// or if(gameObject.CompareTag("YourWallTag"))
				detected=true;
			}
		}
	}
}
