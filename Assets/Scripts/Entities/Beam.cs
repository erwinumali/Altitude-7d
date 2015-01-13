using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]

public class Beam : MonoBehaviour {

	private Animator _animator;

	void Start(){
		_animator = GetComponent<Animator>();	
	}

	void AnimatorBeamEnd(){
		_animator.SetBool("isFiring", false);
	}

}
