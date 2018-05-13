using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

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
    private Rigidbody carRigidBody;
    private GameObject playerBody;

    public int WheelRoattioVisualModifyer = 25;
    public Text speedometer;
    public float maxSpeed = 10f;

    public bool GameOver = false;

    private float ScreenWidth;
    private float currentTurn = 0f;

    private void OnEnable()
    {
        EventManager.LevelFailed += LevelEnded;
        EventManager.LevelWon += LevelEnded;
    }

    private void OnDisable()
    {
        EventManager.LevelFailed -= LevelEnded;
        EventManager.LevelWon -= LevelEnded;
    }

    void Start()
    {
        playerBody = GameObject.Find("Buggy");
        carRigidBody = GameObject.Find("Buggy").GetComponent<Rigidbody>();
        movementDirection = new Vector3(1, 0, 0);
        ScreenWidth = Screen.width;
		Debug.Log ("RunnerLogForGrep            "+Application.platform+" platform detected");
    }

	void Update(){
		if (!GameOver) {
			if (Application.platform == RuntimePlatform.Android)
			{
				currentTurn = CrossPlatformInputManager.GetAxis("Horizontal") * SteeringForce;
				Debug.Log ("RunnerLogForGrep                  turn value: " + currentTurn);
			}
			if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				currentTurn = Input.GetAxis("Horizontal") * SteeringForce;
			}
		}
	}

    void LevelEnded()
    {
        GameOver = true;
        StopPlayerMovement();
    }

    void StopPlayerMovement()
    {
        carRigidBody.isKinematic = true;
        carRigidBody.velocity = Vector3.zero;
    }

    public void RestartPlayer()
    {
        //RighBacktW.brakeTorque = 0;
        //LeftBackW.brakeTorque = 0;
      

        carRigidBody.isKinematic = false;

        GameOver = false;
        playerBody.transform.position = new Vector3(0, 2, 0);
        playerBody.transform.rotation = Quaternion.Euler(Vector3.zero);
        playerBody.transform.Rotate(0, 90, 0);

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameOver)
        {
            RighFrontW.steerAngle = currentTurn;
            LeftFrontW.steerAngle = currentTurn;

			Vector3 colliderPosition;
			Quaternion colliderRotation;
			LeftFrontW.GetWorldPose(out colliderPosition,out colliderRotation);
			LeftFrontWTransform.rotation = colliderRotation;

			RighFrontW.GetWorldPose (out colliderPosition, out colliderRotation);
			RighFrontWTransform.rotation = colliderRotation;

            RighBacktWTransform.Rotate(RighBacktW.rpm / WheelRoattioVisualModifyer / 60 * 360 * Time.deltaTime, 0, 0);
            RighFrontWTransform.Rotate(RighFrontW.rpm / WheelRoattioVisualModifyer / 60 * 360 * Time.deltaTime, 0, 0);
            LeftFrontWTransform.Rotate(LeftFrontW.rpm / WheelRoattioVisualModifyer / 60 * 360 * Time.deltaTime, 0, 0);
            LeftBackWTransform.Rotate(LeftBackW.rpm / WheelRoattioVisualModifyer / 60 * 360 * Time.deltaTime, 0, 0);

            float currentspeed = carRigidBody.velocity.magnitude;

            //2.0f * 3.14f * RighBacktW.radius * RighBacktW.rpm * 60 + "";

            if (currentspeed < maxSpeed)
            {
                RighBacktW.motorTorque = EngineForce * (maxSpeed - currentspeed) / maxSpeed;
                RighBacktW.brakeTorque = 0;
                LeftBackW.motorTorque = EngineForce * (maxSpeed - currentspeed) / maxSpeed;
                LeftBackW.brakeTorque = 0;
            }
            else if (currentspeed > maxSpeed)
            {
                RighBacktW.brakeTorque = EngineForce * BreakForce;
                LeftBackW.brakeTorque = EngineForce * BreakForce;
            }

            speedometer.text = "SPEED: " + Mathf.RoundToInt(currentspeed);

            RighFrontW.steerAngle = currentTurn;
            LeftFrontW.steerAngle = currentTurn;
        }
        else
        {
            //RighBacktW.brakeTorque = Mathf.Infinity;
            //LeftBackW.brakeTorque = Mathf.Infinity;

            //GameObject.Find("Buggy").GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}