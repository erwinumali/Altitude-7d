using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class Player : Character {
	
	protected float _axis;
	
	protected override void Start(){
		base.Start();
		Spawn();
	}
	
	
	protected override void FixedUpdate(){
		_movementVector = Vector2.zero;
		ProcessMovement();	
		
		CheckFront();
		CheckBack();
		
		_rightGroundCheck = CheckGround (transform.localScale.x*0.45f);
		_leftGroundCheck = CheckGround (transform.localScale.x*-0.45f);
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
			Move(DIR_RIGHT, moveSpeed);
		} else if(_axis < 0.0f){
			Move(DIR_LEFT, moveSpeed);
		}
		
		if(Input.GetAxis("Jump") > 0.0f){
			if(_isGrounded){
				_isJumping = true;
				Jump();
			}
		} else {
			_isJumping = false;
		}
	}
	
}
