using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject target;

	public float xOffset = 0;
	public float yOffset = 4.0f;
	
	private Vector3 camVelocity = Vector3.zero;
	
	void Start () {
		if(target == null){
			Debug.LogError("No camera target set!");
			throw new UnityException();
		}
	}
	
	void LateUpdate () {
		/*transform.position = new Vector3(	target.transform.position.x + xOffset, 
											target.transform.position.y + yOffset,
											transform.position.z); 
		*/
		Vector3 destination = new Vector3(	target.transform.position.x + xOffset, 
											target.transform.position.y + yOffset,
											transform.position.z);						
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref camVelocity, 0.2f);
	}
}
