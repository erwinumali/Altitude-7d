using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]

public class Character : MonoBehaviour {
	
	protected readonly float GROUND_TOL = 0.001f;
	protected readonly int DIR_LEFT = -1;
	protected readonly int DIR_RIGHT = 1;

	public int HPMax = 100;
	public int HPCurrent;
	public int armor = 1;
	
	public float moveSpeed = 4.0f;
	public float jumpHeight = 5.0f;
	
	public bool isAlive = false;
	
	protected Bounds _objectBounds;
	
	protected bool isGrounded;
	protected bool isJumping;
	
	protected Vector2 _movementVector;
	protected Vector2 _bottom;
	
	protected float _axis;
	
	void Start(){
		if(HPCurrent >= 0){
			HPCurrent = HPMax;
		}
	}
	
	void Update(){
		if(HPCurrent >= HPMax){
			HPCurrent = HPMax;
		} else if(HPCurrent < 0){
			HPCurrent = 0;
		} 
	}
	
	
	void CheckGround(){
		//_bottom = new Vector2(transform.position.x, transform.position.y - 0.);
		RaycastHit2D res = Physics2D.Raycast(transform.position, -Vector2.up, float.PositiveInfinity, LayerMask.GetMask("Characters"));
		if(res.collider != null){
			Debug.DrawLine(_bottom, res.point, Color.blue);
			float distance = Mathf.Abs(res.point.y - _bottom.y);
			if(distance <= GROUND_TOL && !isJumping){
				isGrounded = true;
			} else {
				isGrounded = false;
			}
			
		} else {
			isGrounded = false;
		}
	}
	
	public virtual void Move(int direction, float speed){
	
	}
	
	public virtual void Jump(){
	
	}
	
	
}
