using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour {

	public GameObject RoadManager;
	public int directionChangingFrequency =3; // Random 0 =  change direction

	private RoadGenerationScript roadScript;
	private int amountOfDirections;

	void Start () {
		roadScript = (RoadGenerationScript)RoadManager.GetComponent<RoadGenerationScript> ();
		if(roadScript==null){
			Debug.Log ("Failed to load roads script. Exiting");
			return;
		}
		amountOfDirections = System.Enum.GetValues (typeof(RoadGenerationScript.Direction)).Length;
		roadScript.InitRoad ();
		AdvancingRoad ();
	}

	private void AdvancingRoad(){
		roadScript.AddTile ();
		if (IsNextTileAnObsticle ()) {
			roadScript.currentDirection = GetNewDirection ();
		}

		Invoke ("AdvancingRoad", 1);
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

	private RoadGenerationScript.Direction GetNewDirection(){
		RoadGenerationScript.Direction result;
		do {
			result = (RoadGenerationScript.Direction)Random.Range (0, amountOfDirections);
		} while (result == roadScript.currentDirection);
		return result;
	}

}
