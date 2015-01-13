using UnityEngine;
using System.Collections;

// contains information about rooms
public class Room : MonoBehaviour {

	public enum RoomLocation { Center, Left, Right };

	// will be checked appropriately by editor designer
	public RoomLocation roomLocation;
	
	public bool hasTopAccess;
	public bool hasBottomAccess;
	public bool hasLeftAccess;
	public bool hasRightAccess;
	
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
