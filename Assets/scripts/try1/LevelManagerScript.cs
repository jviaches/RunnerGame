using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour {

	public GameObject RoadManager;
	public int directionChangingFrequency =3; // Random 0 =  change direction
	public float droneRedrawSpeed = 60f;
	public GameObject Drone;

	public float roadTileCreationSpeed=1;//tile per second


	private RoadGenerationScript roadScript;
	private int amountOfDirections;

	void Start () {
		roadScript = (RoadGenerationScript)RoadManager.GetComponent<RoadGenerationScript> ();
		if(roadScript==null){
			Debug.Log ("Failed to load roads script. Exiting");
			return;
		}
		amountOfDirections = System.Enum.GetValues (typeof(RoadGenerationScript.DirectionFromTo)).Length;

		DroneMovement ();
		AdvancingRoad ();
	}
	private void DroneMovement (){
		Vector3 currentLocation = Drone.transform.position;
		Drone.transform.position = currentLocation + (roadScript.frontWall.transform.position+ new Vector3(0,5,0) - currentLocation)*roadTileCreationSpeed  / droneRedrawSpeed ;
		Drone.transform.Rotate (roadScript.newTileRotation*roadTileCreationSpeed  / droneRedrawSpeed );
		Invoke ("DroneMovement", 1 / droneRedrawSpeed);
	}

	private void AdvancingRoad(){
		roadScript.AdvanceRoad ();


		Invoke ("AdvancingRoad", 1/roadTileCreationSpeed);
	}

	/*
		calculating if next tile should be an obsticle
		for now we are using simple Random, but could use complex function 
		to determine complexity level
	*/
	private bool IsNextTileAnObsticle()
	{
		return Random.Range (0, directionChangingFrequency + 1) == 0;
	}


	}


