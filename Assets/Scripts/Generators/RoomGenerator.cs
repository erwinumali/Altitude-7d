using UnityEngine;
using System.Collections;

public class RoomGenerator : MonoBehaviour {

	public GameObject[] startNodes = new GameObject[3];
	
	public GameObject[] leftRoomPrefabs = new GameObject[3];
	public GameObject[] rightRoomPrefabs = new GameObject[3];
	public GameObject[] centerRoomPrefabs = new GameObject[3];
	
	public float tileSize = 16.0f;

	public int roomWidth = 25;
	public int roomHeight = 36;

	public int towerHeight = 30;
	
	private enum Direction { North, South, East, West };
	
	private int currentHeight;
	private GameObject startNode;
	
	private GameObject currentRoomObject;
	private Room currentRoom;
	
	private GameObject newRoomObject;
	private Room newRoom;
	
	void Start () {
		GenerateRooms();
	}
	
	void Update () {
	
	}
	

	
	void GenerateRooms(){
		currentHeight = 0;
		int randIndex = Random.Range(0,3);
		Direction randDirection = Direction.North;
		
		startNode = startNodes[randIndex];
		
		GameObject[] selectedArray = null;
		if(randIndex == 0){
			selectedArray = leftRoomPrefabs;
		} else if(randIndex == 1){
			selectedArray = centerRoomPrefabs;
		} else if(randIndex == 2){
			selectedArray = rightRoomPrefabs;
		}
		currentRoomObject = PickRandom(selectedArray);
		
		Transform snt = startNode.transform;
		Vector3 sntp = snt.position;
		Debug.Log(startNode.name + " position = " + sntp);
		currentRoomObject = (GameObject)GameObject.Instantiate(currentRoomObject, new Vector3(sntp.x, sntp.y, sntp.z), Quaternion.identity);
		currentRoom = currentRoomObject.GetComponent<Room>();
		currentHeight += 1;
		
		while(true){
			randDirection = PickRandomDirection(true);
				
			while((randDirection == Direction.West  && !currentRoom.hasLeftAccess)  ||
			      (randDirection == Direction.East  && !currentRoom.hasRightAccess) ||
			      (randDirection == Direction.North && !currentRoom.hasTopAccess)   ||
			      (randDirection == Direction.South && !currentRoom.hasBottomAccess) ){
			      	randDirection = PickRandomDirection(true);
			}
			
			newRoomObject = GetNewRoom(GetOppositeDirection(randDirection), PickAdjacentRoomLocation(currentRoom.roomLocation, randDirection));
			newRoom = newRoomObject.GetComponent<Room>();
			
			Transform rmAnchor = GetRoomAnchorAt(newRoom, randDirection).transform;
			Vector3 rmAnchorV = rmAnchor.position;
			
			GameObject.Instantiate(newRoomObject, rmAnchorV, Quaternion.identity); 
			
			if(newRoomObject.transform.position.y != currentRoomObject.transform.position.y){
				currentHeight += 1;
			}
		
		
			if(currentHeight > towerHeight){
				break;
			}
		}
		
	
	}
	
	private GameObject GetNewRoom(Direction openSide, Room.RoomLocation atLocation){
		GameObject ret = null;
		GameObject[] source = null;
		
		switch(atLocation){
			case Room.RoomLocation.Left:
				source = leftRoomPrefabs;
				break;
			case Room.RoomLocation.Right:
				source = rightRoomPrefabs;
				break;
			case Room.RoomLocation.Center:
				source = centerRoomPrefabs;
				break;
			default:
				source = null;
				break;
		}
		
		ret = PickRandom(source);
		Room retRoom = ret.GetComponent<Room>();
		while(	(openSide == Direction.West  && !retRoom.hasLeftAccess)  ||
				(openSide == Direction.East  && !retRoom.hasRightAccess) ||
				(openSide == Direction.North && !retRoom.hasTopAccess)   ||
				(openSide == Direction.South && !retRoom.hasBottomAccess) ){
				ret = PickRandom(source);
				retRoom = ret.GetComponent<Room>();
		}

		return ret;
	}
	
	private GameObject PickRandom(GameObject[] list){
		return list[Random.Range(0, list.Length)];	
	}
	
	private GameObject GetRoomAnchorAt(Room room, Direction dir){
		GameObject ret = null;
		
		switch(dir){
			case Direction.East:
				ret = room.rightAnchor;
				break;
			case Direction.West:
				ret = room.leftAnchor;
				break;
			case Direction.North:
				ret = room.topAnchor;
				break;
			case Direction.South:
				ret = room.bottomAnchor;
				break;	
		}
		
		return ret;
	
	}
	
	private Direction GetOppositeDirection(Direction dir){
		Direction ret = Direction.North;
		
		if(dir == Direction.East) 		ret = Direction.West;
		else if(dir == Direction.West) 	ret = Direction.East;
		else if(dir == Direction.North) ret = Direction.South;
		else if(dir == Direction.South) ret = Direction.North;
		
		return ret;
	}
	
	private Room.RoomLocation PickAdjacentRoomLocation(Room.RoomLocation loc, Direction dir){
		Room.RoomLocation ret = Room.RoomLocation.Center;
		
		switch(loc){
			case Room.RoomLocation.Center:
				if(dir == Direction.East) 		ret = Room.RoomLocation.Right;
				else if(dir == Direction.West) 	ret = Room.RoomLocation.Left;
				else 							ret = loc;
				break;
			case Room.RoomLocation.Left:
			case Room.RoomLocation.Right:
				if(dir == Direction.North || dir == Direction.South) ret = loc;
				else ret = Room.RoomLocation.Center;
				break;
		}
		
		return ret;
	}
	
	private Direction PickRandomDirection(bool exceptBottom){
		int maxVal;
		int rand;
		Direction ret = Direction.North;
		
		if(exceptBottom) maxVal = 3;
		else maxVal = 4;
		
		rand = Random.Range(0, maxVal);
		
		switch(rand){
			case 0:
				ret = Direction.West;
				break;
			case 1:
				ret = Direction.North;
				break;
			case 2:
				ret = Direction.East;
				break;
			case 3:
				ret = Direction.South;
				break;
		}
		
		return ret;
	
	}
	
}
