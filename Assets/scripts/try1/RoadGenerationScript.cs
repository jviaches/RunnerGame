using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerationScript : MonoBehaviour {

	public enum Direction {Left, Up , Right}

	public int straightRoadAmount=3;
	public int turnRightRoadAmount = 2;
	public int turnLeftRoadAmount=2;
	public int directinChangingFrequency =1; // Random 0 =  change direction
	public Direction currentDirection= Direction.Up;
	private Direction previousDirection =Direction.Up;

	private Vector3 newTileLocation;
	private Vector3 newTileRotation;

	public int SimmultaniousRoadTileAmount = 30;

	private Queue<GameObject> Road;


	// Use this for initialization
	void Start () {
		InitRoad ();
		for (int i = 0; i < 120; i++)
			AddTile ();
		
	}
	
	private void InitRoad(){
		Road = new Queue<GameObject> ();
		Road.Enqueue(Instantiate ((GameObject)Resources.Load ("Road/start"),new Vector3(0,0,0) , new Quaternion(0,0,0,0)));
		newTileLocation = new Vector3 (10, 0, 0);
		newTileRotation = new Vector3 (0, 0, 0);
	}

	public void RemoveTile(){
		Road.Dequeue ();
	}
	public void AddTile()
	{
		GameObject newTile;

		switch (currentDirection) {
		case Direction.Left:
			switch (previousDirection){
			case Direction.Left:
				newTile = Instantiate ((GameObject)Resources.Load ("Road/straight/road_s"+Random.Range(1,straightRoadAmount+1)), newTileLocation, Quaternion.LookRotation (new Vector3(0,0,0)));
				newTile.transform.Rotate(newTileRotation);
				newTileLocation = newTileLocation + new Vector3 (0, 0, 10);
				Road.Enqueue (newTile);
				break;

			case Direction.Right://need double left turn
				newTile = Instantiate ((GameObject)Resources.Load ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount+1)), newTileLocation, Quaternion.LookRotation (new Vector3 (0, 0, 0)));
				newTile.transform.Rotate (new Vector3(0,90,0));
				newTileLocation = newTileLocation + new Vector3 (20, 0, -10);
				Road.Enqueue (newTile);
				newTile = Instantiate ((GameObject)Resources.Load ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount+1)), newTileLocation, Quaternion.LookRotation (new Vector3(0,0,0)));
				newTileRotation = new Vector3 (0,-90,0);
				newTileLocation = newTileLocation + new Vector3 (10, 0, 20);
				Road.Enqueue (newTile);
				break;

			default: //direction up
				newTile = Instantiate ((GameObject)Resources.Load ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount+1)), newTileLocation, Quaternion.LookRotation (new Vector3(0,0,0)));
				newTileLocation = newTileLocation + new Vector3 (10, 0, 10);
				newTileRotation = new Vector3 (0,-90,0);
				Road.Enqueue (newTile);
				break;
			}
			break;

		case Direction.Right:
			switch (previousDirection){
			case Direction.Left:
				newTile = Instantiate ((GameObject)Resources.Load ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount+1)), newTileLocation, Quaternion.LookRotation (new Vector3(0,0,0)));
				newTile.transform.Rotate(new Vector3(0,-90,0));
				newTileLocation = newTileLocation + new Vector3 (20, 0, 10);
				Road.Enqueue (newTile);
				newTile = Instantiate ((GameObject)Resources.Load ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount+1)), newTileLocation, Quaternion.LookRotation (new Vector3(0,0,0)));
				newTileLocation = newTileLocation + new Vector3 (10, 0, -20);
				newTileRotation = new Vector3 (0,90,0);
				Road.Enqueue (newTile);
				break;

			case Direction.Right:
				newTile = Instantiate ((GameObject)Resources.Load ("Road/straight/road_s"+Random.Range(1,straightRoadAmount+1)), newTileLocation, Quaternion.LookRotation (new Vector3(0,0,0)));
					newTile.transform.Rotate(newTileRotation);
				newTileLocation = newTileLocation + new Vector3 (0, 0, -10);
				Road.Enqueue (newTile);
				break;

			default: //direction WAS up
				newTile = Instantiate ((GameObject)Resources.Load ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount+1)), newTileLocation, Quaternion.LookRotation (new Vector3(0,0,0)));
				newTileLocation = newTileLocation + new Vector3 (10, 0, -10);
				newTileRotation = new Vector3 (0,90,0);
				Road.Enqueue (newTile);
				break;
			}
			break;

		default: //direction up
			switch (previousDirection){
			case Direction.Left:
				newTile = Instantiate ((GameObject)Resources.Load ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount+1)), newTileLocation, Quaternion.LookRotation (new Vector3(0,0,0)));
				newTile.transform.Rotate(new Vector3(0,-90,0));
				newTileLocation = newTileLocation + new Vector3 (20, 0, 10);
				newTileRotation = new Vector3 (0,0,0);
				Road.Enqueue (newTile);
				break;

			case Direction.Right:
				newTile = Instantiate ((GameObject)Resources.Load ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount+1)), newTileLocation, Quaternion.LookRotation (new Vector3 (0, 0, 0)));
				newTile.transform.Rotate (new Vector3(0,90,0));
				newTileLocation = newTileLocation + new Vector3 (20, 0, -10);
				newTileRotation = new Vector3 (0,0,0);
				Road.Enqueue (newTile);
				break;

			default: //direction was up
				newTile = Instantiate ((GameObject)Resources.Load ("Road/straight/road_s"+Random.Range(1,straightRoadAmount+1)), newTileLocation, Quaternion.LookRotation (new Vector3(0,0,0)));
				newTile.transform.Rotate(newTileRotation);
				newTileLocation = newTileLocation + new Vector3 (10, 0, 0);
				Road.Enqueue (newTile);
				break;
			}
			break;
		}

		previousDirection = currentDirection;
		currentDirection = GenerateTileDirection();
	}

	private Direction GenerateTileDirection(){
		if (Random.Range (0, directinChangingFrequency) != 0)
			return currentDirection;
		var result = (Direction)(Random.Range (0, 3));
		while(result == currentDirection)
			result = (Direction)(Random.Range (0, 3));
		return result;
			
	}
}
