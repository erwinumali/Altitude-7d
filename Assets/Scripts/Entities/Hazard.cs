using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider2D))]

public class Hazard : MonoBehaviour {

	public int damagePerHit = 10;
	public float knockBackStrength = 20.0f;
	public LayerMask affectedEntities = 0;
	
	void Start(){
		collider2D.isTrigger = true;
		if(affectedEntities == 0){
			affectedEntities = 1 << LayerMask.NameToLayer("Player");
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if(((1 << col.gameObject.layer) & affectedEntities) > 0){
			Debug.Log("GOT HIT BRO");
		}
	
	}
}
