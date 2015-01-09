using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class Player : Character {
	
	protected float _axis;
	
	void Start(){
		Spawn();
	}
	
	
	protected override void FixedUpdate(){
		_movementVector = Vector2.zero;
		ProcessMovement();	
		
		base.FixedUpdate();
		ExecuteVector();	
	}
	
	void ProcessMovement(){
		_axis = Input.GetAxis("Horizontal");
		
		if(_axis > 0.0f){
			Move(DIR_RIGHT, moveSpeed);
			_currentDirection = DIR_RIGHT;
		} else if(_axis < 0.0f){
			Move(DIR_LEFT, moveSpeed);
			_currentDirection = DIR_LEFT;
		}
		
		if(Input.GetAxis("Jump") > 0.0f){
			if(isGrounded){
				isJumping = true;
				Jump();
			}
		} else {
			isJumping = false;
		}
	}
	
	void ExecuteVector(){
		Vector2 v = transform.position;
		transform.position = new Vector2(v.x + _movementVector.x, v.y + _movementVector.y);
	}
	
	public override void Move(int direction, float speed){
		Vector2 v = _movementVector;
		_movementVector = new Vector2(v.x + (speed * direction * Time.deltaTime), v.y);
	}
	
	public override void Jump(){
		rigidbody2D.AddForce(Vector2.up * jumpHeight * 200);
	}
}
