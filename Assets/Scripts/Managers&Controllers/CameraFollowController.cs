using UnityEngine;
using System.Collections;

public class CameraFollowController : MonoBehaviour {

	public bool followX = true;
	public bool followY = true;
	
	public float followXPercent = 0.1f;
	public float followYPercent = 0.01f;
	
	private GameObject followCamera = null;

	private float prevCameraX;
	private float prevCameraY;
	
	void Start(){
		followCamera = GameObject.FindGameObjectWithTag("MainCamera");
		if(followCamera == null){
			Debug.LogError("Main Camera not found via tag!");
			throw new UnityException();
		}
	}
	
	void Update () {
		if(followX){
			prevCameraX = followCamera.transform.position.x;
		}
		if(followY){
			prevCameraY = followCamera.transform.position.y;
		}
	}

	void LateUpdate () {
		float x;
		float y;
		if(followX){
			x = transform.position.x + (followCamera.transform.position.x - prevCameraX) * followXPercent;
		} else {
			x = transform.position.x;
		}
		if(followY){
			y = transform.position.y + (followCamera.transform.position.y - prevCameraY) * followYPercent;
		} else {
			y = transform.position.y;
		}
		
		this.transform.position = new Vector3(x, y, transform.position.z);
	}
}
