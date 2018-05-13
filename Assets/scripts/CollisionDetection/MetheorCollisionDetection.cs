using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetheorCollisionDetection : MonoBehaviour {

    public ParticleSystem debrie= null;
    public ParticleSystem smoke = null;
    public ParticleSystem exaust = null;
	public GameObject target = null;

    private LevelManagerScript script_LevelManager;
    private EventManager script_EventManager;
    

    // Use this for initialization
    void Start () {
        script_LevelManager = GameObject.Find("LevelManager").GetComponent<LevelManagerScript>();
        script_EventManager = script_LevelManager.GetEventManagerScript();
	}
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Buggy")
        {
            script_EventManager.FireLevelFailedEvent();
        }
            //if (collision.gameObject.name == "YourWallName")  // or if(gameObject.CompareTag("YourWallTag"))
            Debug.Log("Metheor has landed");
        {
            Rigidbody rb = this.GetComponent<Rigidbody>();
            if(rb!= null)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
            }
            if (debrie != null) debrie.Play();
            
            if (smoke != null) smoke.Play();

            if (exaust != null) exaust.Stop();

			if (target != null)
				target.SetActive (false);
        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}
