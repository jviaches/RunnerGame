using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerationScript : MonoBehaviour {

	public enum Direction {Left, Up , Right}
	public enum WallDirection {TurningLeft,GoingStraight,TurningRight}


	public int turnRightRoadAmount = 2;
	public int turnLeftRoadAmount=2;

	public int leftWallAmount =4;
	public int rightWallAmount = 4;

	public int tileSize = 10;

	public Direction currentDirection= Direction.Up;
	private Direction previousDirection =Direction.Up;

	private Quaternion _originalElementRotation;
	private Vector3 frontWallRotationVector;
	public Vector3 newTileRotation;
	private Vector3 _startingObjectLocation = new Vector3 (-2000, 2000, 0);

	public int SimmultaniousRoadTileAmount = 30;

	private string straighRoadSegmentPath =  "Road/straight/";
	private string straightRoadPrefix = "straight";
	private string leftWallSegmentPath = "walls/left/";
	private string leftWallSegmentPrefix =  "left";
	private string rightWallSegmentPath = "walls/right/";
	private string rightWallSegmentPrefix =  "right";

	private string leftInnerTurnWall = "walls/leftTurnin";
	private string leftOuterTurnWall = "walls/leftTurnOut";
	private string rightInnerTurnWall = "walls/RightTurnin";
	private string rightOuterTurnWall = "walls/RightTurnOut";

	private string tag_roadTile = "road_tile";
	private string tag_rightStraightWall = "right_straight_wall_segment";
	private string tag_leftStraightWall = "left_straight_wall_segment";
	private string tag_leftInnterTurnWall = "left_inner_turnWall_segment";
	private string tag_leftOuterTurnWall = "left_outer_turnWall_segment";
	private string tag_righttInnterTurnWall = "right_inner_turnWall_segment";
	private string tag_rightOuterTurnWall = "right_outer_turnWall_segment";

	private Queue<GameObject> q_road;
	private Queue<GameObject> q_leftSide;
	private Queue<GameObject> q_rightSide;
	public ArrayList q_tileLocations;

	private GameObject frontWall;
	private GameObject backWall;

	private int currentRoadLenght =0;



	private ArrayList pool_roadElements;
	private ArrayList pool_leftWallStraightElemets;
	private ArrayList pool_rightWallStraightElements;
	private ArrayList pool_leftTurnInnerWall;
	private ArrayList pool_leftTurnOuterWall;
	private ArrayList pool_rightTurnOuterWall;
	private ArrayList pool_rightTurnInnerWall;

	// Use this for initialization
	void Start () {
				
	}

	private void InitializeCollections(){
		q_road = new Queue<GameObject> ();
		q_leftSide = new Queue<GameObject> ();
		q_rightSide = new Queue<GameObject> ();
		q_tileLocations = new ArrayList ();

		pool_roadElements = new ArrayList ();
		pool_leftWallStraightElemets = new ArrayList ();
		pool_rightWallStraightElements = new ArrayList ();
		pool_leftTurnInnerWall = new ArrayList ();
		pool_leftTurnOuterWall = new ArrayList ();
		pool_rightTurnInnerWall = new ArrayList ();
		pool_rightTurnOuterWall = new ArrayList ();


		for (int i = 0; i < SimmultaniousRoadTileAmount; i++) {
			//loading elements without veriety
			pool_roadElements.Add(InstanciateObject(straighRoadSegmentPath+straightRoadPrefix,_startingObjectLocation , Vector3.zero,tag_roadTile));
			pool_leftTurnInnerWall.Add (InstanciateObject (leftInnerTurnWall, _startingObjectLocation, Vector3.zero,tag_leftInnterTurnWall));
			pool_leftTurnOuterWall.Add (InstanciateObject (leftOuterTurnWall, _startingObjectLocation, Vector3.zero,tag_leftOuterTurnWall));
			pool_rightTurnInnerWall.Add (InstanciateObject (rightInnerTurnWall,_startingObjectLocation, Vector3.zero,tag_righttInnterTurnWall));
			pool_rightTurnOuterWall.Add (InstanciateObject (rightOuterTurnWall, _startingObjectLocation, Vector3.zero,tag_rightOuterTurnWall));

			//loading elements with veriety
			for (int j = 0; j < leftWallAmount; j++)
				pool_leftWallStraightElemets.Add (InstanciateObject(leftWallSegmentPath+leftWallSegmentPrefix+i,_startingObjectLocation ,Vector3.zero,tag_leftStraightWall));
			for (int j = 0; j < rightWallAmount; j++)
				pool_rightWallStraightElements.Add (InstanciateObject(rightWallSegmentPath+rightWallSegmentPrefix+i,_startingObjectLocation , Vector3.zero,tag_rightStraightWall));
			
		}


	}

	private void InintStartingPoint(){
		
	}

	public void InitRoad(){
		InitializeCollections ();
		_originalElementRotation = ((GameObject)pool_leftWallStraightElemets [0]).transform.rotation;
		InintStartingPoint ();

		frontWall = InstanciateObject ("Road/deadWall", new Vector3 (tileSize, 0, 0), Vector3.zero);
		//backWall = InstanciateObject ("______________", new Vector3 (tileSize, 0, 0), Vector3.zero);
		newTileLocation = new Vector3 (tileSize, 0, 0);


	}

	private void GenerateStraightRoadSegment(Vector3 location, Vector3 direction){
		//must reset object position with ResetGameObject()
		GameObject temp;
		// add road element 
		temp = ResetGameObject((GameObject)pool_roadElements[0]);

		// add left wall element   

		// add right walll leent 


	}

	private void RemoveLastRoadSegment(){
		//we will not reset object on deque - we will do it befor enqueue because of initial position
		GameObject tempObject;
		tempObject = q_leftSide.Dequeue ();
		switch (tempObject.tag) {
		case tag_leftInnterTurnWall:
			pool_leftTurnInnerWall.Add (tempObject);
			break;
		case tag_leftOuterTurnWall:
			pool_leftTurnOuterWall.Add (tempObject);
			break;
		case tag_leftStraightWall:
			pool_leftWallStraightElemets.Add (tempObject);
			break;
		}

		tempObject = q_rightSide.Dequeue ();
		switch (tempObject.tag) {
		case tag_righttInnterTurnWall:
			pool_rightTurnInnerWall.Add (tempObject);
			break;
		case tag_rightOuterTurnWall:
			pool_rightTurnOuterWall.Add (tempObject);
			break;
		case tag_rightStraightWall:
			pool_rightWallStraightElements.Add (tempObject);
			break;
		}
		pool_roadElements.Add (q_road.Dequeue ());
			/* TODO
			backWall.transform.position = ((Vector3) tileLocations[0]);
			StartBackWallAnimation ();
			*/
		q_tileLocations.RemoveAt (0); // removing vector with first road element coordinates

	}
	private GameObject ResetGameObject(GameObject obj){
		if (obj != null) {
			obj.transform.poistion = Vector3.zero;
			obj.transform.rotation = _originalElementRotation;
		}
		return obj;
	}


	private GameObject InstanciateObject(string resourcePath,Vector3 location,Vector3 rotation,string tag){
		GameObject result = Instantiate ((GameObject)Resources.Load (resourcePath), location, Quaternion.LookRotation (Vector3.zero));
		result.tag = tag;
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
				q_road.Enqueue (newTile);
				break;

			case Direction.Right://need double left turn
				newTile =InstanciateObject ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount+1), newTileLocation,new Vector3(0,90,0));


				newTileLocation = newTileLocation + new Vector3 (2*tileSize, 0, -tileSize);

				AddWalls (WallDirection.TurningLeft);
				q_road.Enqueue (newTile);
				newTile =InstanciateObject ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount+1), newTileLocation, Vector3.zero);

				newTileRotation = new Vector3 (0,0,0);
				AddWalls (WallDirection.TurningLeft);
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, 2*tileSize);
				q_road.Enqueue (newTile);
				break;

			default: //direction was up
				newTile = InstanciateObject ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount + 1), newTileLocation, Vector3.zero);
				AddWalls (WallDirection.TurningLeft);
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, 2*tileSize);

				q_road.Enqueue (newTile);
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
				q_road.Enqueue (newTile);
				newTile =InstanciateObject ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount+1), newTileLocation,Vector3.zero);

				newTileRotation = new Vector3 (0,0,0);
				AddWalls (WallDirection.TurningRight);
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, -2*tileSize);

				q_road.Enqueue (newTile);
				break;

			case Direction.Right:
				newTile =InstanciateObject ("Road/straight/road_s"+Random.Range(1,straightRoadAmount+1), newTileLocation, newTileRotation);
				AddWalls (WallDirection.GoingStraight);
				newTileLocation = newTileLocation + new Vector3 (0, 0, -tileSize);
				q_road.Enqueue (newTile);
				break;

			default: //direction WAS up
				newTile =InstanciateObject ("Road/right/turn_right" + Random.Range (1, turnRightRoadAmount+1), newTileLocation,Vector3.zero);
				AddWalls (WallDirection.TurningRight);
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, -2*tileSize);

				q_road.Enqueue (newTile);
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
				q_road.Enqueue (newTile);
				break;

			case Direction.Right:
				newTile = InstanciateObject ("Road/left/turn_left" + Random.Range (1, turnLeftRoadAmount + 1), newTileLocation, new Vector3 (0, 90, 0));

				newTileLocation = newTileLocation + new Vector3 (2*tileSize, 0, -tileSize);
				AddWalls (WallDirection.TurningLeft);
				newTileRotation = new Vector3 (0,0,0);
				q_road.Enqueue (newTile);
				break;

			default: //direction was up
				newTile = InstanciateObject ("Road/straight/road_s" + Random.Range (1, straightRoadAmount + 1), newTileLocation, newTileRotation);
				AddWalls (WallDirection.GoingStraight);
				newTileLocation = newTileLocation + new Vector3 (tileSize, 0, 0);
				q_road.Enqueue (newTile);
				break;
			}
			newTileRotation = new Vector3 (0,0,0);
			break;
		}


		frontWall.transform.position = newTileLocation;

		if (newTileRotation != frontWallRotationVector) {

			frontWall.transform.Rotate (-1*frontWallRotationVector);
			frontWallRotationVector = newTileRotation;
			frontWall.transform.Rotate (newTileRotation);
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
			q_leftSide.Enqueue (InstanciateObject ("walls/left/left"+Random.Range(1,leftWallAmount+1), newTileLocation+shiftVerctor,newTileRotation));
			q_rightSide.Enqueue (InstanciateObject ("walls/right/right"+Random.Range(1,rightWallAmount+1), newTileLocation+-1*shiftVerctor,newTileRotation));
			break;
		case WallDirection.TurningLeft:
			if (newTileRotation.y == 0) {//we were moving up
				q_leftSide.Enqueue (InstanciateObject ("walls/leftTurnOut", newTileLocation + new Vector3 (tileSize, 0, 0), newTileRotation));
				q_rightSide.Enqueue (InstanciateObject ("walls/LeftTurnIn", newTileLocation + new Vector3 (0 , 0, tileSize), newTileRotation));
			} else if (newTileRotation.y == 90) {
				q_leftSide.Enqueue (InstanciateObject ("walls/leftTurnOut", newTileLocation + new Vector3 (-2 * tileSize, 0, 0), new Vector3 (0, 90, 0)));
				q_rightSide.Enqueue (InstanciateObject ("walls/LeftTurnIn", newTileLocation + new Vector3 (-1* tileSize, 0, tileSize), new Vector3 (0, 90, 0)));
			}
			break;
		case WallDirection.TurningRight:
			if (newTileRotation.y == 0) {//we were moving up
				q_leftSide.Enqueue (InstanciateObject ("walls/rightTurnOut", newTileLocation + new Vector3 (tileSize, 0, 0), newTileRotation));
				q_rightSide.Enqueue (InstanciateObject ("walls/RightTurnIn", newTileLocation + new Vector3 (0, 0, -1*tileSize), newTileRotation));
			} else if (newTileRotation.y == -90) { //we were moving left
				q_leftSide.Enqueue (InstanciateObject ("walls/rightTurnOut", newTileLocation + new Vector3 (-2 * tileSize, 0, 0), new Vector3 (0, -90, 0)));
				q_rightSide.Enqueue (InstanciateObject ("walls/RightTurnIn", newTileLocation + new Vector3 (-1* tileSize, 0, -1* tileSize), new Vector3 (0, -90, 0)));
			}
			break;

		}
	}


	private void StartBackWallAnimation(){
		//TODO
	}
}
