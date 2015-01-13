using UnityEngine;
using System.Collections;

// contains information about rooms
public class Room : MonoBehaviour {

	// will be checked appropriately by editor designer
	public bool hasTopAccess;
	public bool hasBottomAccess;
	public bool hasLeftAccess;
	public bool hasRightAccess;
	
	public enum RoomLocation { Center, Left, Right };
	
	public GameObject topRoom;
	public GameObject leftRoom;
	public GameObject rightRoom;
	
	public GameObject topAnchor;
	public GameObject bottomAnchor;
	public GameObject leftAnchor;
	public GameObject rightAnchor;
	
	void Start () {
		
	}
	
	void Update () {
	
	}
	
	
}
