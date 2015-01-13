using UnityEngine;
using System.Collections;

[RequireComponent (typeof (EdgeCollider2D))]

// to be attached to the object containing the edge collider
public class SlimPlatformController : MonoBehaviour {

	public float surfaceCheckDistance = 0.1f;
	public LayerMask surfaceCheckAcknowledges = 0;

	private GameObject player;
	private Player playerScript;

	void Start(){
		if(surfaceCheckAcknowledges == 0){
			surfaceCheckAcknowledges = 1 << LayerMask.NameToLayer("Player");
		}
		
		player = GameObject.FindGameObjectWithTag("Player");
		if(player == null){
			Debug.LogError("GameObject with tag Player not found!");
		} else {
			playerScript = player.GetComponentInChildren<Player>();
		}
		
		collider2D.isTrigger = true;
	}

	void Update () {
		if(!collider2D.isTrigger){
			Vector2 left = new Vector2(transform.position.x + (collider2D.bounds.size.x * -0.5f), collider2D.bounds.center.y + surfaceCheckDistance);
			Vector2 right = new Vector2(transform.position.x + (collider2D.bounds.size.x * 0.5f), collider2D.bounds.center.y + surfaceCheckDistance);
			RaycastHit2D hit = Physics2D.Raycast(left, Vector2.right, Vector2.Distance(collider2D.bounds.min, collider2D.bounds.max), surfaceCheckAcknowledges);
			Debug.DrawLine(left, right, Color.white);
			
			//Debug.Log(hit.point);
			
			if(hit.collider == null){
				if(!collider2D.isTrigger) {
					collider2D.isTrigger = true;
				}
			} else {
				if(hit.collider.tag == "Player" && playerScript.GetVAxis() < 0.0f){
					Physics2D.IgnoreCollision(player.collider2D, this.collider2D);
				} else {
					Physics2D.IgnoreCollision(player.collider2D, this.collider2D, false);
				}
			}
			
		}
	}
	
}
