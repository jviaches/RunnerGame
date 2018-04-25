using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerationScript : MonoBehaviour {

	public enum Direction {Left, Up , Right}

	public int straightRoadAmount=3;
	public int turnRightRoadAmount = 2;
	public int turnLeftRoadAmount=2;
	public int tileSize = 10;

	public Direction currentDirection= Direction.Up;
	private Direction previousDirection =Direction.Up;

	private Vector3 newTileLocation;
	private Vector3 newTileRotation;

	public int SimmultaniousRoadTileAmount = 30;

	private Queue<GameObject> Road;


	// Use this for initialization
	void Start () {
				
	}
	
	public void InitRoad(){
		Road = new Queue<GameObject> ();
		Road.Enqueue(InstanciateObject("Road/start",new Vector3(0,0,0) , new Vector3(0,0,0)));
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
				newTileLocation = newTileLocation + new Vector3 (0, 0, tileSize);
				Road.Enqueue (newTile);
				break;

			case Direction.Right://need double left turn
				newTile =InstanciateObject ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount+1), newTileLocation,new Vector3(0,90,0));
				newTileLocation = newTileLocation + new Vector3 (2*tileSize, 0, -tileSize);
				Road.Enqueue (newTile);
				newTile =InstanciateObject ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount+1), newTileLocation, new Vector3(0,0,0));
				newTileRotation = new Vector3 (0,-90,0);
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, 2*tileSize);
				Road.Enqueue (newTile);
				break;

			default: //direction up
				newTile =InstanciateObject ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount+1), newTileLocation, new Vector3(0,0,0));
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, tileSize);
				newTileRotation = new Vector3 (0,-90,0);
				Road.Enqueue (newTile);
				break;
			}
			break;

		case Direction.Right:
			switch (previousDirection){
			case Direction.Left:
				newTile =InstanciateObject ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount+1), newTileLocation, new Vector3(0,-90,0));
				newTileLocation = newTileLocation + new Vector3 (2*tileSize, 0, tileSize);
				Road.Enqueue (newTile);
				newTile =InstanciateObject ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount+1), newTileLocation,new Vector3(0,0,0));
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, -2*tileSize);
				newTileRotation = new Vector3 (0,90,0);
				Road.Enqueue (newTile);
				break;

			case Direction.Right:
				newTile =InstanciateObject ("Road/straight/road_s"+Random.Range(1,straightRoadAmount+1), newTileLocation, newTileRotation);
				newTileLocation = newTileLocation + new Vector3 (0, 0, -tileSize);
				Road.Enqueue (newTile);
				break;

			default: //direction WAS up
				newTile =InstanciateObject ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount+1), newTileLocation,new Vector3(0,0,0));
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, -tileSize);
				newTileRotation = new Vector3 (0,90,0);
				Road.Enqueue (newTile);
				break;
			}
			break;

		default: //direction up
			switch (previousDirection){
			case Direction.Left:
				newTile =InstanciateObject ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount+1), newTileLocation, new Vector3(0,-90,0));
				newTileLocation = newTileLocation + new Vector3 (2*tileSize, 0, tileSize);
				newTileRotation = new Vector3 (0,0,0);
				Road.Enqueue (newTile);
				break;

			case Direction.Right:
				newTile =InstanciateObject ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount+1), newTileLocation, new Vector3(0,90,0));
				newTileLocation = newTileLocation + new Vector3 (2*tileSize, 0, -tileSize);
				newTileRotation = new Vector3 (0,0,0);
				Road.Enqueue (newTile);
				break;

			default: //direction was up
				newTile =InstanciateObject ("Road/straight/road_s"+Random.Range(1,straightRoadAmount+1), newTileLocation,newTileRotation);
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, 0);
				Road.Enqueue (newTile);
				break;
			}
			break;
		}

		previousDirection = currentDirection;

	}


}
