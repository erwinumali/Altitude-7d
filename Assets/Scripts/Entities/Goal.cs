using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (Collider2D))]
[RequireComponent (typeof (AudioSource))]

public class Goal : MonoBehaviour {
	
	
	private ScoreManager _scoreManagerRef;
	
	void Start () {
		_scoreManagerRef = GameObject.Find("_ScoreManager").GetComponent<ScoreManager>();
	}


	void OnTriggerEnter2D(Collider2D col){
		if(col.tag == "Player"){
			audio.Play();
			//col.gameObject.GetComponent<Player>().isAlive = false;
			renderer.enabled = false;
			transform.FindChild("ScoreUI").gameObject.SetActive(true);
			transform.FindChild("ScoreUI").FindChild("ScoreText").GetComponent<Text>().text = _scoreManagerRef.score.ToString();

		}
	}
}
