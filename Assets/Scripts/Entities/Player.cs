using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
[RequireComponent (typeof (Rigidbody2D))]

public class Player : Character {
	
	void FixedUpdate(){
		_movementVector = Vector2.zero;
		ProcessMovement();	
		
		CheckGround();
		
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
			if(isGrounded){
				isJumping = true;
				Jump();
			}
		} else {
			isJumping = false;
		}
	}
	
	void CheckGround(){
		_bottom = new Vector2(transform.position.x, transform.position.y - 0.9f);
		RaycastHit2D res = Physics2D.Raycast(_bottom, -Vector2.up);
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
