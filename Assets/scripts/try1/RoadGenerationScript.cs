using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerationScript : MonoBehaviour {

	public enum Direction {Left, Up , Right}
	public enum WallDirection {TurningLeft,GoingStraight,TurningRight}

	public int straightRoadAmount=3;
	public int turnRightRoadAmount = 2;
	public int turnLeftRoadAmount=2;

	public int leftWallAmount =4;
	public int rightWallAmount = 4;

	public int tileSize = 10;

	public Direction currentDirection= Direction.Up;
	private Direction previousDirection =Direction.Up;

	public Vector3 newTileLocation;
	public Vector3 newTileRotation;

	public int SimmultaniousRoadTileAmount = 30;

	private Queue<GameObject> Road;
	private Queue<GameObject> leftSide;
	private Queue<GameObject> rightSide;


	// Use this for initialization
	void Start () {
				
	}
	
	public void InitRoad(){
		Road = new Queue<GameObject> ();
		leftSide = new Queue<GameObject> ();
		rightSide = new Queue<GameObject> ();
		Road.Enqueue(InstanciateObject("Road/start",new Vector3(0,0,0) , new Vector3(0,0,0)));
		leftSide.Enqueue (InstanciateObject("walls/start/left",new Vector3(0,0,10) , new Vector3(0,0,0)));
		rightSide.Enqueue (InstanciateObject("walls/start/right",new Vector3(0,0,-10) , new Vector3(0,0,0)));
		newTileLocation = new Vector3 (tileSize, 0, 0);
		newTileRotation = new Vector3 (0, 0, 0);
	}

	public void RemoveTile(){
		Road.Dequeue ();
	}

	private GameObject InstanciateObject(string resourcePath,Vector3 location,Vector3 rotation){
		GameObject result = Instantiate ((GameObject)Resources.Load (resourcePath), location, Quaternion.LookRotation (new Vector3(0,0,0)));
		AddMeshCollider (result);

		result.transform.Rotate (rotation);
		return result;
	}

	private void AddMeshCollider(GameObject obj){
		if (IsAMesh (obj)) {
			obj.AddComponent<MeshCollider> ();
			obj.AddComponent<Rigidbody> ();
			obj.GetComponent<Rigidbody> ().isKinematic = true;
			obj.GetComponent<Rigidbody> ().useGravity = false;
		}
		for (int i = 0; i < obj.transform.childCount; i++) {
			AddMeshCollider (obj.transform.GetChild (i).gameObject);
		}
	}

	private bool IsAMesh(GameObject obj){
		return obj.GetComponent<MeshFilter> () != null;
	}

	public void AddTile()
	{
		GameObject newTile;

		switch (currentDirection) {
		case Direction.Left:
			switch (previousDirection){
			case Direction.Left:
				newTile = InstanciateObject("Road/straight/road_s"+Random.Range(1,straightRoadAmount+1),newTileLocation,newTileRotation);
				AddWalls (WallDirection.GoingStraight);
				newTileLocation = newTileLocation + new Vector3 (0, 0, tileSize);
				Road.Enqueue (newTile);
				break;

			case Direction.Right://need double left turn
				newTile =InstanciateObject ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount+1), newTileLocation,new Vector3(0,90,0));


				newTileLocation = newTileLocation + new Vector3 (2*tileSize, 0, -tileSize);

				AddWalls (WallDirection.TurningLeft);
				Road.Enqueue (newTile);
				newTile =InstanciateObject ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount+1), newTileLocation, new Vector3(0,0,0));

				newTileRotation = new Vector3 (0,0,0);
				AddWalls (WallDirection.TurningLeft);
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, 2*tileSize);
				Road.Enqueue (newTile);
				break;

			default: //direction was up
				newTile = InstanciateObject ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount + 1), newTileLocation, new Vector3 (0, 0, 0));
				AddWalls (WallDirection.TurningLeft);
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, 2*tileSize);

				Road.Enqueue (newTile);
				break;
			}
			newTileRotation = new Vector3 (0,-90,0);
			break;

		case Direction.Right:
			switch (previousDirection){
			case Direction.Left:
				newTile =InstanciateObject ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount+1), newTileLocation, new Vector3(0,-90,0));
				newTileLocation = newTileLocation + new Vector3 (2*tileSize, 0, tileSize);
				AddWalls (WallDirection.TurningRight);
				Road.Enqueue (newTile);
				newTile =InstanciateObject ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount+1), newTileLocation,new Vector3(0,0,0));

				newTileRotation = new Vector3 (0,0,0);
				AddWalls (WallDirection.TurningRight);
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, -2*tileSize);

				Road.Enqueue (newTile);
				break;

			case Direction.Right:
				newTile =InstanciateObject ("Road/straight/road_s"+Random.Range(1,straightRoadAmount+1), newTileLocation, newTileRotation);
				AddWalls (WallDirection.GoingStraight);
				newTileLocation = newTileLocation + new Vector3 (0, 0, -tileSize);
				Road.Enqueue (newTile);
				break;

			default: //direction WAS up
				newTile =InstanciateObject ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount+1), newTileLocation,new Vector3(0,0,0));
				AddWalls (WallDirection.TurningRight);
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, -2*tileSize);

				Road.Enqueue (newTile);
				break;
			}
			newTileRotation = new Vector3 (0,90,0);
			break;

		default: //direction up
			switch (previousDirection){
			case Direction.Left:
				newTile = InstanciateObject ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount + 1), newTileLocation, new Vector3 (0, -90, 0));
				newTileLocation = newTileLocation + new Vector3 (2 * tileSize, 0, tileSize);
				AddWalls (WallDirection.TurningRight);
				newTileRotation = new Vector3 (0,0,0);
				Road.Enqueue (newTile);
				break;

			case Direction.Right:
				newTile = InstanciateObject ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount + 1), newTileLocation, new Vector3 (0, 90, 0));

				newTileLocation = newTileLocation + new Vector3 (2*tileSize, 0, -tileSize);
				AddWalls (WallDirection.TurningLeft);
				newTileRotation = new Vector3 (0,0,0);
				Road.Enqueue (newTile);
				break;

			default: //direction was up
				newTile = InstanciateObject ("Road/straight/road_s" + Random.Range (1, straightRoadAmount + 1), newTileLocation, newTileRotation);
				AddWalls (WallDirection.GoingStraight);
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, 0);
				Road.Enqueue (newTile);
				break;
			}
			newTileRotation = new Vector3 (0,0,0);
			break;
		}

	

		previousDirection = currentDirection;

	}

	private void AddWalls(WallDirection Direction){

		switch (Direction) {

		case WallDirection.GoingStraight:
			Vector3 shiftVerctor = new Vector3(0,0,tileSize);;
			if(newTileRotation.y == 0) //road is moving up
				shiftVerctor = new Vector3(0,0,tileSize);
			else  // road is moving left or right
				shiftVerctor = new Vector3(tileSize*newTileRotation.y/90,0,0);
			leftSide.Enqueue (InstanciateObject ("walls/left/left"+Random.Range(1,leftWallAmount+1), newTileLocation+shiftVerctor,newTileRotation));
			rightSide.Enqueue (InstanciateObject ("walls/right/right"+Random.Range(1,rightWallAmount+1), newTileLocation+-1*shiftVerctor,newTileRotation));
			break;
		case WallDirection.TurningLeft:
			if (newTileRotation.y == 0) {//we were moving up
				leftSide.Enqueue (InstanciateObject ("walls/leftTurnOut", newTileLocation + new Vector3 (tileSize, 0, 0), newTileRotation));
				rightSide.Enqueue (InstanciateObject ("walls/LeftTurnIn", newTileLocation + new Vector3 (0 , 0, tileSize), newTileRotation));
			} else if (newTileRotation.y == 90) {
				leftSide.Enqueue (InstanciateObject ("walls/leftTurnOut", newTileLocation + new Vector3 (-2 * tileSize, 0, 0), new Vector3 (0, 90, 0)));
				rightSide.Enqueue (InstanciateObject ("walls/LeftTurnIn", newTileLocation + new Vector3 (-1* tileSize, 0, tileSize), new Vector3 (0, 90, 0)));
			}
			break;
		case WallDirection.TurningRight:
			if (newTileRotation.y == 0) {//we were moving up
				leftSide.Enqueue (InstanciateObject ("walls/rightTurnOut", newTileLocation + new Vector3 (tileSize, 0, 0), newTileRotation));
				rightSide.Enqueue (InstanciateObject ("walls/RightTurnIn", newTileLocation + new Vector3 (0, 0, -1*tileSize), newTileRotation));
			} else if (newTileRotation.y == -90) { //we were moving left
				leftSide.Enqueue (InstanciateObject ("walls/rightTurnOut", newTileLocation + new Vector3 (-2 * tileSize, 0, 0), new Vector3 (0, -90, 0)));
				rightSide.Enqueue (InstanciateObject ("walls/RightTurnIn", newTileLocation + new Vector3 (-1* tileSize, 0, -1* tileSize), new Vector3 (0, -90, 0)));
			}
			break;

		}
	}
}
