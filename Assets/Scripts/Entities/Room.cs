using UnityEngine;
using System.Collections;

// contains information about rooms
public class Room : MonoBehaviour {

	// will be checked appropriately by editor designer
	public bool hasTopAccess;
	public bool hasBottomAccess;
	public bool hasLeftAccess;
	public bool hasRightAccess;
	
	public Room topRoom;
	public Room leftRoom;
	public Room rightRoom;
	
	void Start () {
	
	}
	
	void Update () {
	
	}
}
