using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountdown : MonoBehaviour {

	[SerializeField] Text CountdownUI;
	[SerializeField] int countdownLimit;
	float timer;

	public virtual void OnEnable () {
		timer = countdownLimit;
	}

	void Update(){
		if (timer > 0)
			timer -= Time.deltaTime;
		else 
		{
			Destroy(gameObject);
			LevelManager.isCountdownOver = true;
		}

		UpdateCountdownUI(((int)timer + 1).ToString());
	}

	void UpdateCountdownUI(string s)
	{
		CountdownUI.text = s;
	}
}
