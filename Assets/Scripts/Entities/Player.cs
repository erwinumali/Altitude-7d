using UnityEngine;
using System.Collections;

//[RequireComponent (typeof (BoxCollider2D))]

public class Player : Character {
	
	protected float _hAxis;
	protected float _vAxis;
	
	private float originalGravity;
	private float reducedGravity = 4.0f;
	
	protected override void Start(){
		base.Start();
		originalGravity = rigidbody2D.gravityScale;
		Spawn();
	}
	
	protected override void Update(){
		_movementVector = Vector2.zero;
		animator.SetBool("isMoving", false);
		animator.SetBool("isGrounded", _isGrounded);
		ProcessMovement();	
	
		CheckFront();
		CheckBack();
		
		_rightGroundCheck = CheckGround (collider2D.bounds.size.x * 0.3f);
		_leftGroundCheck = CheckGround (-collider2D.bounds.size.x * 0.3f);
		if(_rightGroundCheck || _leftGroundCheck){
			_isGrounded = true;
		} else {
			_isGrounded = false;
		}
		ExecuteVector();
		
		LimitYVelocity();
	
	}
	
	void ProcessMovement(){
		_hAxis = Input.GetAxis("Horizontal");
		_vAxis = Input.GetAxis("Vertical");
		
		if(_hAxis > 0.0f){
			Move(DIR_FRONT, moveSpeed);
		} else if(_hAxis < 0.0f){
			Move(DIR_BACK, moveSpeed);
		}
		
		if(Input.GetAxis("Jump") > 0.0f || _vAxis > 0.0f){
			if(_isGrounded){
				_isJumping = true;
				Jump();
			} else {
				rigidbody2D.gravityScale = reducedGravity;
			}
		} else {
			rigidbody2D.gravityScale = originalGravity;
			_isJumping = false;
		}
		
		
	}
	
	public float GetVAxis(){
		return _vAxis;
	}
	
}
