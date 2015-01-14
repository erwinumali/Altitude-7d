using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	Text hpText;
	Image hpBar;
	
	string hpTextValue;
	float hpRatio;
	float hpBarSmoothing = 0.10f;
	Vector3 hpBarBackRef = Vector3.zero;
	
	private Player player;
	
	void Start () {
		hpText = GetComponentInChildren<Text>();
		hpBar = GetComponentInChildren<Image>();
		
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if(player.HPCurrent <= 0){
			hpTextValue = "0";
		} else {
			hpTextValue = player.HPCurrent.ToString();
		}
		hpText.text = hpTextValue;
		
		
		hpRatio = (float)player.HPCurrent / (float)player.HPMax;
		if(hpRatio < 0) hpRatio = 0;
		 
		Vector3 dampedValue = new Vector3(	hpBar.transform.localScale.x, 
		                                  	hpRatio, 
		                                  	hpBar.transform.localScale.z); 
		hpBar.transform.localScale = Vector3.SmoothDamp(hpBar.transform.localScale, dampedValue, ref hpBarBackRef, hpBarSmoothing);
	}
}
