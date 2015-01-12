using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider2D))]

public class Hazard : MonoBehaviour {

	public int damagePerHit = 10;
	public float tickDuration = 1.0f;
	public float knockbackStrength = 20.0f;
	public LayerMask affectedEntities = 0;

	private float timer;
	
	void Start(){
		collider2D.isTrigger = true;
		if(affectedEntities == 0){
			affectedEntities = 1 << LayerMask.NameToLayer("Player");
		}
	}
	
	// uses bitwise operation to check if object entering is in list
	void OnTriggerEnter2D(Collider2D col){
		if(((1 << col.gameObject.layer) & affectedEntities) > 0){
			Character c = col.GetComponent<Character>();
			c.HPCurrent -= damagePerHit;
			c.rigidbody2D.velocity.Normalize();
			c.rigidbody2D.AddForce(-c.rigidbody2D.velocity * knockbackStrength);
		}
	}
	
	void OnTriggerStay2D(Collider2D col){
		timer += Time.deltaTime;
		if(timer >= tickDuration){
			OnTriggerEnter2D(col);
			timer = 0;
		}
	}
	
	void OnTriggerExit2D(Collider2D col){
		timer = 0;
	}
}
