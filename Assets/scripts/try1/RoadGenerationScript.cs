using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerationScript : MonoBehaviour {
									
	public enum DirectionFromTo {UpUp, UpLeft,UpRight,LeftUp,LeftLeft,LeftRight,RightUp, RightLeft , RightRight}

	private Vector3 rotationVector_HorizontalLeft =  new Vector3 (0, 90, 0);
	private Vector3 rotationVector_HorizontalRight = new  Vector3 (0 ,- 90, 0);


	public int leftWallAmount =4;
	public int rightWallAmount = 4;
	public int tileSize = 10;
	public int turnsPerTwoSegments=5;//basicaly - frequency of turns



	private Quaternion _originalElementRotation;
	private Vector3 frontWallRotationVector;
	public Vector3 newTileRotation;
	private Vector3 _startingObjectLocation = new Vector3 (-2000, 2000, 0);

	public int SimmultaniousRoadTileAmount = 30;

	private string straighRoadSegmentPath =  "Road/straight/";
	private string straightRoadPrefix = "straight";
	private string turnRoadSegmentPath = "Road/left/";
	private string turnRoadSegmentPrefix="turnLeft";
	private string leftWallSegmentPath = "walls/left/";
	private string leftWallSegmentPrefix =  "left";
	private string rightWallSegmentPath = "walls/right/";
	private string rightWallSegmentPrefix =  "right";

	private string leftInnerTurnWall = "walls/leftTurnin";
	private string leftOuterTurnWall = "walls/leftTurnOut";
	private string rightInnerTurnWall = "walls/RightTurnin";
	private string rightOuterTurnWall = "walls/RightTurnOut";

	private string tag_fronWall = "front_wal";
	private string tag_roadTile = "road_tile";
	private string tag_roadTurnTile = "road_turn_tile";
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

	public GameObject frontWall;
	private GameObject backWall;

	private int _currentRoadLenght =0;
	private DirectionFromTo _previousDirection;


	private ArrayList pool_roadElements;
	private ArrayList pool_roadTurnElements;
	private ArrayList pool_leftWallStraightElemets;
	private ArrayList pool_rightWallStraightElements;
	private ArrayList pool_leftTurnInnerWall;
	private ArrayList pool_leftTurnOuterWall;
	private ArrayList pool_rightTurnOuterWall;
	private ArrayList pool_rightTurnInnerWall;

	private bool isInitialized = false;

	// Use this for initialization
	void Start () {
		if(!isInitialized){
			InitializeCollections ();	
			InitRoad ();
		}
	}
	public void ForceStart(){
		if(!isInitialized){
			InitializeCollections ();	
			InitRoad ();
		}
	}
	private void InitializeCollections(){
		q_road = new Queue<GameObject> ();
		q_leftSide = new Queue<GameObject> ();
		q_rightSide = new Queue<GameObject> ();
		q_tileLocations = new ArrayList ();

		pool_roadElements = new ArrayList ();
		pool_roadTurnElements = new ArrayList ();
		pool_leftWallStraightElemets = new ArrayList ();
		pool_rightWallStraightElements = new ArrayList ();
		pool_leftTurnInnerWall = new ArrayList ();
		pool_leftTurnOuterWall = new ArrayList ();
		pool_rightTurnInnerWall = new ArrayList ();
		pool_rightTurnOuterWall = new ArrayList ();


		for (int i = 0; i < SimmultaniousRoadTileAmount+10; i++) {
			//loading elements without veriety
			pool_roadElements.Add(InstanciateObject(straighRoadSegmentPath+straightRoadPrefix,_startingObjectLocation , Vector3.zero,tag_roadTile));
			pool_roadTurnElements.Add(InstanciateObject(turnRoadSegmentPath+turnRoadSegmentPrefix,_startingObjectLocation , Vector3.zero,tag_roadTurnTile));
			pool_leftTurnInnerWall.Add (InstanciateObject (leftInnerTurnWall, _startingObjectLocation, Vector3.zero,tag_leftInnterTurnWall));
			pool_leftTurnOuterWall.Add (InstanciateObject (leftOuterTurnWall, _startingObjectLocation, Vector3.zero,tag_leftOuterTurnWall));
			pool_rightTurnInnerWall.Add (InstanciateObject (rightInnerTurnWall,_startingObjectLocation, Vector3.zero,tag_righttInnterTurnWall));
			pool_rightTurnOuterWall.Add (InstanciateObject (rightOuterTurnWall, _startingObjectLocation, Vector3.zero,tag_rightOuterTurnWall));

			//loading elements with veriety
			for (int j = 1; j < leftWallAmount+1; j++)
				pool_leftWallStraightElemets.Add (InstanciateObject(leftWallSegmentPath+leftWallSegmentPrefix+j,_startingObjectLocation ,Vector3.zero,tag_leftStraightWall));
			for (int j = 1; j < rightWallAmount+1; j++)
				pool_rightWallStraightElements.Add (InstanciateObject(rightWallSegmentPath+rightWallSegmentPrefix+j,_startingObjectLocation , Vector3.zero,tag_rightStraightWall));
			
		}


	}

	private GameObject ConvertLeftTurnToRightTurnROAD(GameObject obj){
		if (obj == null)
			return null;
		obj.transform.Rotate(rotationVector_HorizontalRight);
		return obj;
	}

	private void InintStartingPoint(){
		AddNextRoadSegment (DirectionFromTo.UpUp);
	}

	public void InitRoad(){
		_previousDirection = DirectionFromTo.UpUp;
		frontWall = InstanciateObject ("Road/deadWall", Vector3.zero, Vector3.zero,tag_fronWall);
		//backWall = InstanciateObject ("______________", new Vector3 (tileSize, 0, 0), Vector3.zero);
		_originalElementRotation = ((GameObject)pool_leftWallStraightElemets [0]).transform.rotation;

		InintStartingPoint ();

		isInitialized = true;
	
	}

	public void AdvanceRoad(){
		AddNextRoadSegment (ChoosingNewDirection ());
	}

	private DirectionFromTo ChoosingNewDirection(){
		//return DirectionFromTo.UpUp;
		DirectionFromTo result = _previousDirection;

		//checking if we need to change direction
		if (Random.Range (0, turnsPerTwoSegments) == 0) {
			int new_direction = Random.Range (0, 3);
			result = (DirectionFromTo)(((int)_previousDirection % 3) * 3 + new_direction);
		}
		return result;
	}

	private void GenerateLeftTurnRoadSegment(DirectionFromTo direction, Vector3 location){
		GameObject tempObject;
		if (direction == DirectionFromTo.UpLeft) {
			// road
			tempObject = (GameObject)pool_roadTurnElements[0];
			pool_roadTurnElements.RemoveAt (0);
			tempObject = ResetGameObject (tempObject);
			tempObject.transform.position = location;
			q_road.Enqueue (tempObject);
			//left wall
			tempObject = (GameObject)pool_leftTurnInnerWall[0];
			pool_leftTurnInnerWall.RemoveAt (0);
			tempObject = ResetGameObject (tempObject);
			tempObject.transform.position = location + new Vector3 (-1 * tileSize, 0, tileSize);
			q_leftSide.Enqueue (tempObject);
			//right wall
			tempObject = (GameObject)pool_leftTurnOuterWall[0];
			pool_leftTurnOuterWall.RemoveAt (0);
			tempObject = ResetGameObject (tempObject);
			tempObject.transform.position = location;
			q_rightSide.Enqueue (tempObject);
		
		} else if (direction == DirectionFromTo.RightUp) {
			// road
			tempObject = (GameObject)pool_roadTurnElements[0];
			pool_roadTurnElements.RemoveAt (0);
			tempObject = ResetGameObject (tempObject);
			tempObject.transform.position = location;
			tempObject.transform.Rotate (new Vector3 (0, 90, 0));
			q_road.Enqueue (tempObject);
			//left wall
			tempObject = (GameObject)pool_leftTurnInnerWall[0];
			pool_leftTurnInnerWall.RemoveAt (0);
			tempObject = ResetGameObject (tempObject);
			tempObject.transform.position = location + new Vector3 ( tileSize, 0, tileSize);
			tempObject.transform.Rotate (new Vector3 (0, 90, 0));
			q_leftSide.Enqueue (tempObject); 
			//right wall
			tempObject = (GameObject)pool_leftTurnOuterWall[0];
			pool_leftTurnOuterWall.RemoveAt (0);
			tempObject = ResetGameObject (tempObject);
			tempObject.transform.position = location;
			tempObject.transform.Rotate (new Vector3 (0, 90, 0));
			q_rightSide.Enqueue (tempObject); 
		}
	}

	private void GenerateRightTurnRoadSegment(DirectionFromTo direction, Vector3 location){
		GameObject tempObject;

		if (direction == DirectionFromTo.UpRight) {
			// add road 
			tempObject = (GameObject)pool_roadTurnElements[0];
			pool_roadTurnElements.RemoveAt (0);
			tempObject = ResetGameObject (tempObject);
			tempObject = ConvertLeftTurnToRightTurnROAD (tempObject);
			tempObject.transform.position = location;
			q_road.Enqueue (tempObject);
			//add inner -right wall
			tempObject = (GameObject)pool_rightTurnInnerWall[0];
			pool_rightTurnInnerWall.RemoveAt (0);
			tempObject = ResetGameObject (tempObject);
			tempObject.transform.position = location + new Vector3 ( -1*tileSize, 0, -1*tileSize);
			q_rightSide.Enqueue (tempObject); 
			//add outer - left wall
			tempObject = (GameObject)pool_rightTurnOuterWall[0];
			pool_rightTurnOuterWall.RemoveAt (0);
			tempObject = ResetGameObject (tempObject);
			tempObject.transform.position = location;
			q_leftSide.Enqueue (tempObject); 
		}

		if (direction == DirectionFromTo.LeftUp) {
			// add road 
			tempObject =(GameObject) pool_roadTurnElements[0];
			pool_roadTurnElements.RemoveAt (0);
			tempObject = ResetGameObject (tempObject);
			tempObject = ConvertLeftTurnToRightTurnROAD (tempObject);
			tempObject.transform.Rotate (rotationVector_HorizontalRight);
			tempObject.transform.position = location;
			q_road.Enqueue (tempObject);
			//add inner -right wall
			tempObject = (GameObject)pool_rightTurnInnerWall[0];
			pool_rightTurnInnerWall.RemoveAt (0);
			tempObject = ResetGameObject (tempObject);
			tempObject.transform.Rotate (rotationVector_HorizontalRight);
			tempObject.transform.position = location + new Vector3 ( tileSize, 0, -1*tileSize);
			q_rightSide.Enqueue (tempObject); 
			//add outer - left wall
			tempObject = (GameObject)pool_rightTurnOuterWall[0];
			pool_rightTurnOuterWall.RemoveAt (0);
			tempObject = ResetGameObject (tempObject);
			tempObject.transform.Rotate (rotationVector_HorizontalRight);
			tempObject.transform.position = location;
			q_leftSide.Enqueue (tempObject); 


		}
	}

	private void AddNextRoadSegment(DirectionFromTo direction){
		Vector3 newLocation = frontWall == null ? new Vector3(0,0,0): frontWall.transform.position;
		if(_currentRoadLenght >= SimmultaniousRoadTileAmount){
			RemoveLastRoadSegment ();
			_currentRoadLenght = _currentRoadLenght - 1;
			if ((direction == DirectionFromTo.LeftRight) || (direction == DirectionFromTo.RightLeft)) {
				RemoveLastRoadSegment ();
				_currentRoadLenght = _currentRoadLenght - 1;
			}
		}
		Debug.Log (direction);
		switch(direction){
		case DirectionFromTo.UpUp:
			frontWall = ResetGameObject (frontWall);
			frontWall.transform.position = newLocation + new Vector3 (tileSize, 0, 0);
			GenerateStraightRoadSegmentAt (newLocation, Vector3.zero);
			q_tileLocations.Add (newLocation);
			
			break;
		case DirectionFromTo.UpLeft:
			frontWall = ResetGameObject (frontWall);
			frontWall.transform.position = newLocation + new Vector3 (tileSize, 0, 2 * tileSize);
			frontWall.transform.Rotate (-1 * rotationVector_HorizontalLeft);
			newLocation = newLocation + new Vector3 (tileSize, 0, 0);
			GenerateLeftTurnRoadSegment (DirectionFromTo.UpLeft, newLocation);
			q_tileLocations.Add (newLocation);
			direction = DirectionFromTo.LeftLeft;
			break;
		case DirectionFromTo.UpRight:
			frontWall = ResetGameObject (frontWall);
			frontWall.transform.position = newLocation + new Vector3 (tileSize, 0, -2 * tileSize);
			frontWall.transform.Rotate (new Vector3 (0, 90, 0));
			newLocation = newLocation + new Vector3 (tileSize, 0, 0);
			GenerateRightTurnRoadSegment (DirectionFromTo.UpRight, newLocation);
			q_tileLocations.Add (newLocation);
			direction = DirectionFromTo.RightRight;
			break;
		case DirectionFromTo.LeftUp:
			frontWall = ResetGameObject (frontWall);
			frontWall.transform.position = newLocation + new Vector3 (2 * tileSize, 0, tileSize);
			//frontWall.transform.Rotate (new Vector3 (0, -90, 0));
			newLocation = newLocation + new Vector3 (0, 0, tileSize);
			GenerateRightTurnRoadSegment (DirectionFromTo.LeftUp, newLocation);
			q_tileLocations.Add (newLocation);
			direction = DirectionFromTo.UpUp;
			break;
		case DirectionFromTo.LeftLeft:
			frontWall.transform.position = newLocation + new Vector3 (0, 0, tileSize);
			GenerateStraightRoadSegmentAt (newLocation, rotationVector_HorizontalLeft);
			q_tileLocations.Add (newLocation);
			break;
		case DirectionFromTo.LeftRight:
			frontWall = ResetGameObject (frontWall);
			frontWall.transform.position = newLocation + new Vector3 (3 * tileSize, 0, -1 * tileSize);
			frontWall.transform.Rotate (new Vector3(0,90,0));

			newLocation = newLocation + new Vector3 (0, 0, tileSize);
			GenerateRightTurnRoadSegment (DirectionFromTo.LeftUp, newLocation);
			q_tileLocations.Add (newLocation);
			newLocation = newLocation + new Vector3 (3 * tileSize, 0, 0);
			GenerateRightTurnRoadSegment (DirectionFromTo.UpRight, newLocation);
			q_tileLocations.Add (newLocation);
			_currentRoadLenght=_currentRoadLenght+1;
			direction = DirectionFromTo.RightRight;
			break;
		case DirectionFromTo.RightUp:
			frontWall = ResetGameObject (frontWall);
			frontWall.transform.position = newLocation + new Vector3 (2 * tileSize, 0, -1 * tileSize);
			newLocation = newLocation + new Vector3 (0, 0, -1 * tileSize);
			GenerateLeftTurnRoadSegment (DirectionFromTo.RightUp, newLocation);
			q_tileLocations.Add (newLocation);
			direction = DirectionFromTo.UpUp;
			break;
		case DirectionFromTo.RightLeft:

			frontWall = ResetGameObject (frontWall);
			frontWall.transform.position = newLocation + new Vector3 (3*tileSize, 0, tileSize);
			frontWall.transform.Rotate (-1*rotationVector_HorizontalLeft);
			//right->up
			newLocation = newLocation + new Vector3 (0, 0, -1 * tileSize);
			GenerateLeftTurnRoadSegment (DirectionFromTo.RightUp, newLocation);
			q_tileLocations.Add (newLocation);
			_currentRoadLenght = _currentRoadLenght + 1;
			//up-> left

			newLocation = newLocation + new Vector3 (3*tileSize, 0, 0);
			GenerateLeftTurnRoadSegment (DirectionFromTo.UpLeft, newLocation);
			q_tileLocations.Add (newLocation);
			direction = DirectionFromTo.LeftLeft;
			break;
		case DirectionFromTo.RightRight:
			frontWall.transform.position = newLocation + new Vector3 (0,0, -1*tileSize);
			GenerateStraightRoadSegmentAt (newLocation, rotationVector_HorizontalRight);
			q_tileLocations.Add (newLocation);
			break;
		}
				_currentRoadLenght=_currentRoadLenght+1;
		_previousDirection = direction;
	}

	private void GenerateStraightRoadSegmentAt(Vector3 location, Vector3 rotation){
		//must reset object position with ResetGameObject()
		Vector3 leftOffset = new Vector3(0,0,tileSize);
		Vector3 rightOffset = new Vector3(0,0,-1*tileSize);

		// add road element 
		AddElelementFromPoolToQueue(pool_roadElements,q_road,location,rotation,true);

		if (rotation.y > 0) { // left orientatin
			
			// add left wall  element   
			AddElelementFromPoolToQueue (pool_leftWallStraightElemets, q_leftSide, location + new Vector3(-1*tileSize,0,0), -1*rotation, true);
			// add right walll leent 
			AddElelementFromPoolToQueue (pool_rightWallStraightElements, q_rightSide, location +  new Vector3(tileSize,0,0), -1*rotation, true);
		} else if (rotation.y < 0) { // right orientation
			
			// add left wall  element   
			AddElelementFromPoolToQueue (pool_leftWallStraightElemets, q_leftSide, location + new Vector3(tileSize,0,0), -1*rotation, true);
			// add right walll leent 
			AddElelementFromPoolToQueue (pool_rightWallStraightElements, q_rightSide, location + new Vector3(-1*tileSize,0,0), -1*rotation, true);
		} else { //goig straight

			// add left wall  element   
			AddElelementFromPoolToQueue (pool_leftWallStraightElemets, q_leftSide, location + leftOffset, rotation, true);
			// add right walll leent 
			AddElelementFromPoolToQueue (pool_rightWallStraightElements, q_rightSide, location + rightOffset, rotation, true);
		}

		q_tileLocations.Add (location);
	}

	private void AddElelementFromPoolToQueue(ArrayList pool, Queue<GameObject> q ,Vector3 location, Vector3 rotation , bool needsReset){
		GameObject temp;
		Debug.Log ("Adding element from pool amount of objects in pool"+pool.Count+" to queue with amount "+q.Count);
		temp = (GameObject)pool[0];
		pool.RemoveAt (0);
		if (needsReset)
			temp = ResetGameObject (temp);
		temp.transform.position = location;
		temp.transform.Rotate (rotation);
		q.Enqueue (temp);
	}

	private void RemoveLastRoadSegment(){
		//we will not reset object on deque - we will do it befor enqueue because of initial position
		GameObject tempObject;
		tempObject = q_leftSide.Dequeue ();
		if (tempObject.name== tag_leftInnterTurnWall)
			pool_leftTurnInnerWall.Add (tempObject);

		else if (tempObject.name== tag_leftOuterTurnWall)
			pool_leftTurnOuterWall.Add (tempObject);

		else if (tempObject.name== tag_leftStraightWall)
			pool_leftWallStraightElemets.Add (tempObject);


		tempObject.transform.position = _startingObjectLocation;

		tempObject = q_rightSide.Dequeue ();

		if (tempObject.name== tag_righttInnterTurnWall)
			pool_rightTurnInnerWall.Add (tempObject);

		else if (tempObject.name== tag_rightOuterTurnWall)
			pool_rightTurnOuterWall.Add (tempObject);
		
		else if (tempObject.name== tag_rightStraightWall)
			pool_rightWallStraightElements.Add (tempObject);
		tempObject.transform.position = _startingObjectLocation;

		tempObject = q_road.Dequeue ();
		if (tempObject.name == tag_roadTurnTile)
			pool_roadTurnElements.Add (tempObject);
		else if (tempObject.name == tag_roadTile)
			pool_roadElements.Add (tempObject);
		
		tempObject.transform.position = _startingObjectLocation;
		

			/* TODO
			backWall.transform.position = ((Vector3) tileLocations[0]);
			StartBackWallAnimation ();
			*/
		q_tileLocations.RemoveAt (0); // removing vector with first road element coordinates

	}
	private GameObject ResetGameObject(GameObject obj){
		if (obj != null) {
			obj.transform.position = Vector3.zero;
			obj.transform.rotation = _originalElementRotation;
		}
		return obj;
	}


	private GameObject InstanciateObject(string resourcePath,Vector3 location,Vector3 rotation,string tag){
		GameObject result = null;;
		try{
		 result = Instantiate ((GameObject)Resources.Load (resourcePath), location, Quaternion.LookRotation (Vector3.zero));
		}
		catch (System.Exception e) {
			Debug.Log (e);
		}
		if (result != null) {
			result.name = tag;
			AddMeshCollider (result);

			result.transform.Rotate (rotation);
		}
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


		private void StartBackWallAnimation(){
		//TODO
	}
}
