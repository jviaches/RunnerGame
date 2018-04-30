using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour {

	public float EngineForce = 200f;
	public float SteeringForce = 65f;
	public float BreakForce = 1f;
	public bool IsMoving = false;
	public WheelCollider RighFrontW;
	public WheelCollider RighBacktW;
	public WheelCollider LeftFrontW;
	public WheelCollider LeftBackW;

	public Vector3 movementDirection;
	public GameObject playerObject;

	private Rigidbody playersBody;
	private float _moveZ;


	private LevelManagerScript lmScript;
	public bool GameOver = false;

	void Start () {

		lmScript =  GameObject.Find("LevelManager").GetComponent<LevelManagerScript>();
		movementDirection = new Vector3 (1, 0, 0);


	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find("PlayerBody").transform.position.y < -1) {
			lmScript.FinishLevel (true);
		}
		if (!GameOver) {
			float v = Input.GetAxis ("Vertical") * EngineForce;
			float h = Input.GetAxis ("Horizontal") * SteeringForce;

			RighBacktW.motorTorque = v;
			LeftBackW.motorTorque = v;

			RighFrontW.steerAngle = h;
			LeftFrontW.steerAngle = h;

			//RighFrontW.motorTorque = v;
			//LeftFrontW.motorTorque = v;

		} else {
			RighBacktW.brakeTorque=Mathf.Infinity;
			LeftBackW.brakeTorque=Mathf.Infinity;

			GameObject.Find ("PlayerBody").GetComponent<Rigidbody> ().isKinematic = true;

		}
	}




}
