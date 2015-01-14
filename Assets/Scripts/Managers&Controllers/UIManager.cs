using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public Text hpText;
	public Image hpBar;
	public Text scoreText;
	
	string hpTextValue;
	float hpRatio;
	float hpBarSmoothing = 0.10f;
	Vector3 hpBarBackRef = Vector3.zero;
	
	private Player _player;
	private ScoreManager _scoreManagerRef;
	
	void Start () {
		if(hpText == null || hpBar == null || scoreText == null){
			Debug.LogError("Incomplete UI elements assigned!");
		} 
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		_scoreManagerRef = GameObject.Find("_ScoreManager").GetComponent<ScoreManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_player.HPCurrent <= 0){
			hpTextValue = "0";
		} else {
			hpTextValue = _player.HPCurrent.ToString();
		}
		hpText.text = hpTextValue;
		
		hpRatio = (float)_player.HPCurrent / (float)_player.HPMax;
		if(hpRatio < 0) hpRatio = 0;
		 
		Vector3 dampedValue = new Vector3(	hpBar.transform.localScale.x, 
		                                  	hpRatio, 
		                                  	hpBar.transform.localScale.z); 
		hpBar.transform.localScale = Vector3.SmoothDamp(hpBar.transform.localScale, dampedValue, ref hpBarBackRef, hpBarSmoothing);
		
		scoreText.text = _scoreManagerRef.score.ToString();
	}
}
