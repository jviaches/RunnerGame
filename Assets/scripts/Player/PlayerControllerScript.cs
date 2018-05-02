using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public float EngineForce = 200f;
    public float SteeringForce = 65f;
    public float BreakForce = 1f;

    public WheelCollider RighFrontW;
    public WheelCollider RighBacktW;
    public WheelCollider LeftFrontW;
    public WheelCollider LeftBackW;

	public Transform RighFrontWTransform;
	public Transform RighBacktWTransform;
	public Transform LeftFrontWTransform;
	public Transform LeftBackWTransform;

    public Vector3 movementDirection;
	private int currentSpint = 0;

	public int WheelRPM= 200;

    public bool GameOver = false;

    private LevelManagerScript levelManagerScript;
   

    void Start()
    {
        levelManagerScript = GameObject.Find("LevelManager").GetComponent<LevelManagerScript>();
        movementDirection = new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameObject.Find("Buggy").transform.position.y < -1)
            levelManagerScript.FinishLevel(true);

        if (!GameOver)
        {
            float v = Input.GetAxis("Vertical") * EngineForce;
            float h = Input.GetAxis("Horizontal") * SteeringForce;


			RighBacktWTransform.Rotate (RighBacktW.rpm / 60  * 360 * Time.deltaTime, 0, 0);
			RighFrontWTransform.Rotate (RighFrontW.rpm / 60 * 360 * Time.deltaTime, 0, 0);
			LeftFrontWTransform.Rotate (LeftFrontW.rpm / 60 * 360 * Time.deltaTime, 0, 0);
			LeftBackWTransform.Rotate (LeftBackW.rpm  / 60  * 360 * Time.deltaTime, 0, 0);

            RighBacktW.motorTorque = v;
            LeftBackW.motorTorque = v;

            RighFrontW.steerAngle = h;
            LeftFrontW.steerAngle = h;
        }
        else
        {
            RighBacktW.brakeTorque = Mathf.Infinity;
            LeftBackW.brakeTorque = Mathf.Infinity;

            GameObject.Find("Buggy").GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}