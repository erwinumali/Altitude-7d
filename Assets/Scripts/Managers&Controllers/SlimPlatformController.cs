using UnityEngine;
using System.Collections;

[RequireComponent (typeof (EdgeCollider2D))]

// to be attached to the object containing the edge collider
public class SlimPlatformController : MonoBehaviour {

	public float surfaceCheckDistance = 0.1f;
	public LayerMask surfaceCheckAcknowledges = 0;

	void Start(){
		if(surfaceCheckAcknowledges == 0){
			surfaceCheckAcknowledges = 1 << LayerMask.NameToLayer("Player");
		}
		collider2D.isTrigger = true;
	}

	void Update () {
		if(!collider2D.isTrigger){
			Vector2 left = new Vector2(transform.position.x + (collider2D.bounds.size.x * -0.5f), collider2D.bounds.center.y + surfaceCheckDistance);
			Vector2 right = new Vector2(transform.position.x + (collider2D.bounds.size.x * 0.5f), collider2D.bounds.center.y + surfaceCheckDistance);
			RaycastHit2D hit = Physics2D.Raycast(left, right, Vector2.Distance(right, left), surfaceCheckAcknowledges);
			Debug.DrawLine(left, right, Color.white);
			

			
			if(hit.collider == null){
				collider2D.isTrigger = true;
			} else {
				Debug.Log("HIT " + hit.transform);
			}
			
		}
	}
	
}
