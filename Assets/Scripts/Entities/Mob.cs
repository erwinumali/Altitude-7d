using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class Mob : Character {

	public enum MobBehavior { IdleStill, IdleMove, SeekAndAttack, RandomJumping }
	
	public MobBehavior mobBehavior = MobBehavior.IdleMove; 
	public float behaviorTick = 2.0f;
	public string mobName = "Welkin Tern";
	
	private bool isRunningBehavior = false;
	private float timer = 0;
	
	protected override void Start(){
		base.Start();
		Spawn();
	}
	
	protected override void Update(){
		if(isAlive){
			base.Update();
			timer += Time.deltaTime;
			if(timer > behaviorTick){
				if(mobBehavior == MobBehavior.IdleMove){
					int direction = Random.Range(DIR_LEFT,DIR_RIGHT + 1);
					if(direction != 0){
						if(direction == DIR_LEFT && _leftGroundCheck || direction == DIR_RIGHT && _rightGroundCheck){
							Debug.Log(direction);
							StartCoroutine(IdleMove (direction));
							timer = 0;
						} else {	// repeat
							timer = behaviorTick;
						}
					}
				}
			}
		}
	}
	
	protected override void FixedUpdate(){
		CheckFront();
		CheckBack();
		_rightGroundCheck = CheckGround (transform.localScale.x*0.45f);
		_leftGroundCheck = CheckGround (transform.localScale.x*-0.45f);
	}
	
	IEnumerator IdleMove(int direction){
		float time = 0;
		while(time < 1.0f){
			_movementVector = Vector2.zero;
			if((direction == DIR_LEFT && _leftGroundCheck) || (direction == DIR_RIGHT && _rightGroundCheck)){
				Move (direction, moveSpeed);
				ExecuteVector();
			} else {
				break;
			}
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}	
	}
	
}
