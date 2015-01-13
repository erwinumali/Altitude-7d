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
	private GameObject currentRoom;
	
	void Start () {
		GenerateRooms();
	}
	
	void Update () {
	
	}
	

	
	void GenerateRooms(){
		currentHeight = 0;
		startNode = startNodes[1];
		// assume center
		
		currentRoom = PickRandom(centerRoomPrefabs);
		
		Transform snt = startNode.transform;
		Vector3 sntp = snt.position;
		Debug.Log(startNode.name + " position = " + sntp);
		GameObject.Instantiate(currentRoom, new Vector3(sntp.x, sntp.y, sntp.z), Quaternion.identity);
	
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
	
	
}
