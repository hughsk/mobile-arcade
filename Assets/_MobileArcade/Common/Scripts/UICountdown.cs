using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountdown : MonoBehaviour {

	[SerializeField] Text CountdownUI;
	[SerializeField] int countdownLimit;
	RectTransform xform;
	float timer;

	public virtual void OnEnable () {
		timer = countdownLimit;
		xform = GetComponent<RectTransform>();
	}

	void Update(){
		if (timer > -1)
			timer -= Time.deltaTime;
		else
		{
			Destroy(gameObject);
		}

		LevelManager.isCountdownOver = timer <= 0f;
		float t = 1f - Mathf.Repeat(Mathf.Max(-1f, timer), 1f);

		xform.localScale = Vector3.one * EasingFunction.EaseInOutExpo(1f, 0f, t);
		xform.localRotation = Quaternion.Euler(0, 0, EasingFunction.EaseInOutCirc(+30f, -80f, t));

		UpdateCountdownUI(timer >= 0 ? ((int)timer + 1).ToString() : "GO!");
	}

	void UpdateCountdownUI(string s)
	{
		CountdownUI.text = s;
	}
}
