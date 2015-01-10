using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]

public class Character : MonoBehaviour {
	
	protected readonly float GROUND_TOL = 0.1f;
	protected readonly int DIR_BACK = -1;
	protected readonly int DIR_FRONT = 1;

	public int HPMax = 100;
	public int HPCurrent;
	public int armor = 1;
	
	public float moveSpeed = 4.0f;
	public float jumpHeight = 5.0f;
	
	public float frontSeekDistance = 20.0f;
	public float backSeekDistance = 10.0f;
	
	public bool isAlive = false;
	
	public LayerMask groundCheckIgnores = 0;
	
	public bool showDebugGizmos = true;
	
	protected bool _isGrounded = false;
	protected bool _isJumping = false;
	
	protected bool _rightGroundCheck = false;
	protected bool _leftGroundCheck = false;
	
	protected Vector2 _movementVector;
	
	protected int _currentDirection;
	
	protected virtual void Start(){
		if(groundCheckIgnores == 0){
			groundCheckIgnores = 1 << LayerMask.NameToLayer("Player");
		}
	}
	
	protected virtual void Update(){
		CheckInspectorValues();
		
		_movementVector = Vector2.zero;
		CheckFront();
		CheckBack();
		CheckGround();
		ExecuteVector();
	}
	
	protected virtual void FixedUpdate(){

	}
	
	protected void CheckInspectorValues(){
		if(HPCurrent >= HPMax){
			HPCurrent = HPMax;
		} else if(HPCurrent < 0){
			HPCurrent = 0;
		} 
		
		if(frontSeekDistance < 0){
			frontSeekDistance = 0;
		}
		if(backSeekDistance < 0){
			backSeekDistance = 0;
		}
	}
	
	public virtual void Spawn(){
		_currentDirection = DIR_FRONT;
		if(HPCurrent >= 0){
			HPCurrent = HPMax;
		}
		isAlive = true;
	}
	
	public virtual void Die(){
		HPCurrent = 0;
		isAlive = false;
	}
	
	public virtual void Move(int direction, float speed){
		Vector2 v = _movementVector;
		_movementVector = new Vector2(v.x + (speed * direction * Time.deltaTime), v.y);
		_currentDirection = direction;
	}
	
	public virtual void Jump(){
		rigidbody2D.AddForce(Vector2.up * jumpHeight * 200);
	}
	
	protected RaycastHit2D[] CheckFront(){
		return CheckSide("front");
	}
	
	protected RaycastHit2D[] CheckBack(){
		return CheckSide("back");
	}
	
	protected RaycastHit2D[] CheckSide(string side){
		float sideModifier = 1.0f;
		float seekDistance = frontSeekDistance;
		if(side == "front"){
			if(_currentDirection == DIR_BACK){
				sideModifier = -1.0f;
				seekDistance = backSeekDistance;
			}
		} else if(side == "back"){
			if(_currentDirection == DIR_FRONT){
				sideModifier = -1.0f;
				seekDistance = backSeekDistance;
			}
		} else {
			Debug.LogError("Invalid string input for CheckSide!");
			return null;
		}
		
		RaycastHit2D[] res = 
		Physics2D.RaycastAll(transform.position, Vector2.right * _currentDirection * sideModifier, seekDistance);
		if(res.Length > 1){
			foreach(RaycastHit2D col in res){
				if(col.collider != null && col.collider != this.collider2D){
					if(showDebugGizmos) Debug.DrawLine(transform.position, col.point, Color.cyan);
					//Debug.Log ("I hit " + col.collider.gameObject.name + " at the " + side);
				}
			}		
		} else {
			if(showDebugGizmos) Debug.DrawLine(	transform.position, 
							new Vector3(transform.position.x  + (seekDistance * _currentDirection * sideModifier), transform.position.y, transform.position.z),
							Color.green); 
		}
		
		return res;
	}
	
	protected bool CheckGround(){
		return CheckGround(0.0f, true);
	}
	
	protected bool CheckGround(float centerOffset){
		return CheckGround(centerOffset, false);
	}
	
	protected bool CheckGround(float centerOffset, bool alterGroundedState){
		bool retVal = false;
		Vector2 castSource = new Vector2(transform.position.x + centerOffset, transform.position.y);
		RaycastHit2D res = Physics2D.Raycast(castSource, -Vector2.up, Mathf.Infinity, ~(groundCheckIgnores));
		if(res.collider != null){
			if(showDebugGizmos) Debug.DrawLine(castSource, res.point, Color.blue);
			float distance = Mathf.Abs(res.point.y - transform.position.y);
			distance -= this.collider2D.bounds.size.y * 0.5f;	//approx. halve the collider size
			if(distance <= GROUND_TOL && !_isJumping){
				retVal = true;
			} else {
				retVal = false;
			}
			
		} else {
			retVal = false;
		}
		
		if(alterGroundedState){
			_isGrounded = retVal;
		}
		
		return retVal;
	}
	
	protected virtual void ExecuteVector(){
		Vector2 v = transform.position;
		transform.position = new Vector2(v.x + _movementVector.x, v.y + _movementVector.y);
		//rigidbody2D.AddForce(new Vector2(_movementVector.x, _movementVector.y) * 100.0f);
	}
	
}
