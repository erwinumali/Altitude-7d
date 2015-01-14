using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (Collider2D))]
[RequireComponent (typeof (AudioSource))]

public class Pickup : MonoBehaviour {

	public enum PickupType {HPUp, ArmorUp};
	
	public PickupType pickupType = PickupType.HPUp;
	public int pickupValue = 20;
	public LayerMask canBePickedUpBy = 0;
	
	private bool _hasBeenPickedUp = false;
	
	
	void Start () {
		if(canBePickedUpBy == 0){
			canBePickedUpBy = 1 << LayerMask.NameToLayer("Player");
		}
	}

	// uses bitwise operation to check if object entering is in list
	void OnTriggerEnter2D(Collider2D col){
		if(((1 << col.gameObject.layer) & canBePickedUpBy) > 0 && !_hasBeenPickedUp){
			if(pickupType == PickupType.HPUp){
				Character c = col.GetComponent<Character>();
				c.Heal(pickupValue);
				audio.Play();
				
				renderer.enabled = false;
				_hasBeenPickedUp = true;;
			}
		}
	}
}
