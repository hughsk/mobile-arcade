using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(RectTransform))]
public class UICycle : MonoBehaviour {
	RectTransform rect;
	Vector3 position;
	Text text;

	void OnEnable () {
		text = GetComponent<Text>();
		rect = GetComponent<RectTransform>();
		position = rect.localPosition;
	}

	static float time = 0f;

	void Update () {
		time += Time.deltaTime;

		float t = Mathf.Repeat(time, 30f) / 10f;

		t = Mathf.Clamp01(t);
		bool down = t > 0.5f;
		t = Mathf.Max(0, t * 5f) * Mathf.Min(1f, 5f - t * 5f);
		t = Mathf.Clamp01(t);
		t = down
			? EasingFunction.EaseInExpo(0f, 1f, t)
			: EasingFunction.EaseOutExpo(1f, 0f, 1f - t);

		text.color = new Color(1, 1, 1, t);
		rect.localPosition = position + Vector3.up * Mathf.Sin(time * 10f) * 8f;
	}
}
