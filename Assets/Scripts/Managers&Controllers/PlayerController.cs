using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// to be refactored to Player class
	public float moveSpeed = 4.0f;
	public float jumpHeight = 10.0f;
	private bool isGrounded = false;
	private bool isJumping = false;
	
	
	public float gravity = 15.0f;


	private readonly int DIR_LEFT = -1;
	private readonly int DIR_RIGHT = 1;
	
	private readonly float TOL = 0.001f;
	
	private Vector2 _movementVector;
	private Vector2 _bottomPlayer;
	
	private float _axis;

	void Start () {

	}
	
	void Update () {


	}
	
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
		_bottomPlayer = new Vector2(transform.position.x, transform.position.y - 0.9f);
		RaycastHit2D res = Physics2D.Raycast(_bottomPlayer, -Vector2.up);
		if(res.collider != null){
			Debug.DrawLine(_bottomPlayer, res.point, Color.blue);
			float distance = Mathf.Abs(res.point.y - _bottomPlayer.y);
			if(distance <= TOL && !isJumping){
				Vector2 v = _movementVector;
				_movementVector = new Vector2(	v.x,
												0.0f);
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
		transform.position = new Vector2(	v.x + _movementVector.x,
											v.y + _movementVector.y);
	}
	
	void Move(int direction, float speed){
		Vector2 v = _movementVector;
		_movementVector = new Vector2(	v.x + (speed * direction * Time.deltaTime), 
										v.y);

	}
	
	void Jump(){
		rigidbody2D.AddForce(Vector2.up * jumpHeight * 200);
	}
}
