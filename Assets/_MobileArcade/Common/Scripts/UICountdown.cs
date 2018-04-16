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
		if (timer > 0)
			timer -= Time.deltaTime;
		else
		{
			Destroy(gameObject);
			LevelManager.isCountdownOver = true;
		}

		float t = 1f - Mathf.Repeat(Mathf.Max(0, timer), 1f);

		xform.localScale = Vector3.one * EasingFunction.EaseInOutExpo(1f, 0f, t);
		xform.localRotation = Quaternion.Euler(0, 0, EasingFunction.EaseInOutCirc(+30f, -80f, t));

		UpdateCountdownUI(((int)timer + 1).ToString());
	}

	void UpdateCountdownUI(string s)
	{
		CountdownUI.text = s;
	}
}
