using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]

public class Character : MonoBehaviour {
	
	protected readonly float GROUND_TOL = 0.1f;
	protected readonly int DIR_LEFT = -1;
	protected readonly int DIR_RIGHT = 1;

	public int HPMax = 100;
	public int HPCurrent;
	public int armor = 1;
	
	public float moveSpeed = 4.0f;
	public float jumpHeight = 5.0f;
	
	public bool isAlive = false;
	
	protected bool isGrounded = false;
	protected bool isJumping = false;
	
	protected Vector2 _movementVector;
	
	protected int _currentDirection;
	
	protected virtual void Start(){
		_currentDirection = DIR_RIGHT;
	}
	
	protected virtual void Update(){
		if(HPCurrent >= HPMax){
			HPCurrent = HPMax;
		} else if(HPCurrent < 0){
			HPCurrent = 0;
		} 
	}
	
	protected virtual void FixedUpdate(){
		CheckFront();
		CheckGround();
	}
	
	public virtual void Spawn(){
		if(HPCurrent >= 0){
			HPCurrent = HPMax;
		}
	}
	
	public virtual void Move(int direction, float speed){
		
	}
	
	public virtual void Jump(){
		
	}
	
	protected void CheckFront(){
		RaycastHit2D[] res = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector2.right * _currentDirection), 10.0f);
		foreach(RaycastHit2D col in res){
			if(col.collider != null && col.collider != this.collider2D){
				Debug.DrawLine(transform.position, col.point, Color.red);
				//Debug.Log ("I hit " + col.collider.gameObject.name + " at the front");
			}
		}
	}
	
	protected void CheckBack(){
	

	}
	
	protected void CheckGround(){
		RaycastHit2D res = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Characters")));
		if(res.collider != null){
			Debug.DrawLine(transform.position, res.point, Color.blue);
			float distance = Mathf.Abs(res.point.y - transform.position.y);
			
			distance -= transform.localScale.y * 0.5f;
			if(distance <= GROUND_TOL && !isJumping){
				isGrounded = true;
			} else {
				isGrounded = false;
			}
			
		} else {
			isGrounded = false;
		}

	}
	

	
	
}
