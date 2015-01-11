using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class Player : Character {
	
	protected float _axis;
	private float originalGravity;
	private float reducedGravity = 4.0f;
	
	protected override void Start(){
		base.Start();
		originalGravity = rigidbody2D.gravityScale;
		Spawn();
	}
	
	protected override void Update(){
		_movementVector = Vector2.zero;
		ProcessMovement();	
		
		CheckFront();
		CheckBack();
		
		_rightGroundCheck = CheckGround (collider2D.bounds.size.x * 0.45f);
		_leftGroundCheck = CheckGround (-collider2D.bounds.size.x * 0.45f);
		if(_rightGroundCheck || _leftGroundCheck){
			_isGrounded = true;
		} else {
			_isGrounded = false;
		}
		ExecuteVector();
	
	}
	
	void ProcessMovement(){
		_axis = Input.GetAxis("Horizontal");
		
		if(_axis > 0.0f){
			Move(DIR_FRONT, moveSpeed);
		} else if(_axis < 0.0f){
			Move(DIR_BACK, moveSpeed);
		}
		
		if(Input.GetAxis("Jump") > 0.0f){
			rigidbody2D.gravityScale = reducedGravity;
			if(_isGrounded){
				_isJumping = true;
				Jump();
			}
		} else {
			rigidbody2D.gravityScale = originalGravity;
			_isJumping = false;
		}
	}
	
}
