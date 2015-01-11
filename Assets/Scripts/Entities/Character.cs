using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Animator))]

public class Character : MonoBehaviour {
	
	protected readonly float GROUND_TOL = 0.1f;			// ground tolerance check
	protected readonly float JUMP_MODIFIER = 200.0f;	// arbitrary height correction factor
	protected readonly int DIR_BACK = -1;
	protected readonly int DIR_FRONT = 1;

	public int HPMax = 100;
	public int HPCurrent;
	public int armor = 1;
	
	public float moveSpeed = 4.0f;
	public float jumpHeight = 5.0f;
	
	public float frontSeekDistance = 20.0f;				// how far can the char see?
	public float backSeekDistance = 10.0f;
	
	public bool isAlive = false;
	
	public LayerMask groundCheckIgnores = 0;			// placeholder; check Inspector!
	
	public bool showDebugGizmos = true;
	
	protected bool _isGrounded = false;
	protected bool _isJumping = false;
	
	protected bool _rightGroundCheck = false;
	protected bool _leftGroundCheck = false;
	
	protected Vector2 _movementVector;
	
	protected int _currentDirection;
	
	protected Animator animator;
	
	protected virtual void Start(){
		if(groundCheckIgnores == 0){
			groundCheckIgnores = 1 << LayerMask.NameToLayer("Player");
		}
		animator = GetComponent<Animator>();
	}
	
	protected virtual void Update(){
		CheckInspectorValues();
		animator.SetBool("isMoving", false);
		animator.SetBool("isGrounded", _isGrounded);
		
		_movementVector = Vector2.zero;
		CheckFront();
		CheckBack();
		CheckGround();
		ExecuteVector();
	}
	
	// related actions when character spawns
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
	
	// GOTCHA: does not actually move the character as soon as the method exits;
	// instead adjusts the _movementVector variable; you need to use ExecuteVector()
	// to actually move the character
	public virtual void Move(int direction, float speed){
		Vector2 v = _movementVector;
		_movementVector = new Vector2(v.x + (speed * direction * Time.deltaTime), v.y);
		
		if(_currentDirection != direction){
			transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
		} 
		animator.SetBool("isMoving", true);
		
		_currentDirection = direction;
		
	}
	
	// JUMP_MODIFIER a completely arbitrary variable to tweak jump height;
	// refer to top of code to adjust
	public virtual void Jump(){
		rigidbody2D.AddForce(Vector2.up * jumpHeight * JUMP_MODIFIER);
	}
	
	protected RaycastHit2D[] CheckFront(){
		return CheckSide("front");
	}
	
	protected RaycastHit2D[] CheckBack(){
		return CheckSide("back");
	}
	
	// returns array of objects in range (defined by public variables) of the side
	// specified. Direction (front/back) is relative the character's current
	// facing direction
	// GOTCHA: can 'see' through walls and otherwise opaque objects
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
				}
			}		
		} else {
			if(showDebugGizmos) {
				Debug.DrawLine(	transform.position, 
								new Vector3(transform.position.x + (seekDistance * _currentDirection * sideModifier), 
											transform.position.y, 
											transform.position.z),
								Color.green);
			}
		}
		
		return res;
	}
	
	// overload convenience methods
	protected bool CheckGround(){
		return CheckGround(0.0f, true);
	}
	
	protected bool CheckGround(float centerOffset){
		return CheckGround(centerOffset, false);
	}
	
	// raycasts to ground; if hit collider isn't in ignored list, then you're standing
	protected bool CheckGround(float centerOffset, bool alterGroundedState){
		bool retVal = false;
		Vector2 castSource = new Vector2(transform.position.x + centerOffset, transform.position.y);
		RaycastHit2D res = Physics2D.Raycast(castSource, -Vector2.up, Mathf.Infinity, ~(groundCheckIgnores));
		
		if(res.collider != null){
			if(showDebugGizmos) Debug.DrawLine(castSource, res.point, Color.blue);
			float distance = Mathf.Abs(res.point.y - transform.position.y);
			distance -= this.collider2D.bounds.size.y * 0.5f;	// approx. halve the collider size
			
			if(distance <= GROUND_TOL && !_isJumping){
				if(res.collider.gameObject.layer == LayerMask.NameToLayer("LevelSlimPlatforms")){
					res.collider.isTrigger = false;				// partial support for half platforms
				}
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
	
	// execute movement vector; must be called after all move-related methods
	protected virtual void ExecuteVector(){
		Vector2 v = transform.position;
		transform.position = new Vector2(v.x + _movementVector.x, v.y + _movementVector.y);
	}
	
	// checks if inspector values are valid; can be called every start of 
	// update, but not really mandatory. Used to avoid editor errors
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
	
}
