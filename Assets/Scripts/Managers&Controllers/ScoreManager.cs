using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public int score;

	void Start () {
		score = 0;
	}
	
	public void AddScore(int value){
		score += value;
	}
}
