using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class Player : Character {
	
	public int fireDamage = 34;
	public float fireRange = 5.0f;
	
	public LayerMask shotOnlyAffects = 0;
	
	public float resetDuration = 1.5f;
	
	protected float _hAxis;
	protected float _vAxis;
	protected float _fireAxis;	// hehe
	protected float _chargeAxis;
	protected float _resetAxis;
	
	protected bool _needsToCharge = false;
	protected bool _hasFired = false;
	
	private float _originalGravity;
	private float _reducedGravity = 4.0f;
	
	private Animator _beamAnimator;
	private Color _origChargedColor;
	
	private RaycastHit2D[] shotHit;
	
	private float _resetTimer;
	
	protected override void Start(){
		base.Start();
		_originalGravity = rigidbody2D.gravityScale;
		if(shotOnlyAffects == 0){
			shotOnlyAffects = 1 << LayerMask.NameToLayer("MobTopVulnerable");
		}
		
		_beamAnimator = transform.GetChild(0).GetComponent<Animator>();
		_origChargedColor = _sr.color;
		
		Spawn();
	}
	
	protected override void Update(){
		if(isAlive){
			_movementVector = Vector2.zero;
			animator.SetBool("isMoving", false);
	
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
			
			animator.SetBool("isGrounded", _isGrounded);
			CheckIfDead();
		} else {
			animator.SetBool("isMoving", false);
		}
		
	}
	
	public override void Spawn(){
		base.Spawn();
		_needsToCharge = false;
	
	}
	
	void ProcessMovement(){
		_hAxis = Input.GetAxis("Horizontal");
		_vAxis = Input.GetAxis("Vertical");
		_fireAxis = Input.GetAxis("Fire");
		_chargeAxis = Input.GetAxis("Charge");
		_resetAxis = Input.GetAxis("Reset");
		
		Debug.Log(_resetTimer);
		if(_resetAxis > 0.0f){
			_resetTimer += Time.deltaTime;
			if(_resetTimer >= resetDuration){
				Application.LoadLevel(0);
			}
		} else {
			_resetTimer = 0;
		}
		
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
				rigidbody2D.gravityScale = _reducedGravity;
			}
		} else {
			rigidbody2D.gravityScale = _originalGravity;
			_isJumping = false;
		}
		
		if(_fireAxis > 0.0f && !_hasFired){
			if(showDebugGizmos) Debug.DrawLine(transform.position, new Vector3(transform.position.x + (fireRange * (float)_currentDirection), transform.position.y, transform.position.z), Color.black);
			Fire();
			
		}
		if(_fireAxis <= 0){
			_hasFired = false;
		}
		
		
		if(_chargeAxis > 0.0f){
			Charge();
		}
		
	}
	
	void Fire(){
		if(!_needsToCharge){
			shotHit = Physics2D.RaycastAll(transform.position, Vector2.right * _currentDirection, fireRange, shotOnlyAffects);
			
			foreach(RaycastHit2D hit in shotHit){
				Debug.Log("Damaged " + hit.collider.gameObject.name + "!");
				hit.collider.GetComponent<Character>().Damage(fireDamage);
			}
			
			_beamAnimator.SetBool("isFiring", true);
			_hasFired = true;
			_needsToCharge = true; 
			
			_sr.color = Color.gray;
		}
	}

	
	void Charge(){
		_needsToCharge = false;
		_sr.color = _origChargedColor;
	}

	
	public float GetVAxis(){
		return _vAxis;
	}
	
}
