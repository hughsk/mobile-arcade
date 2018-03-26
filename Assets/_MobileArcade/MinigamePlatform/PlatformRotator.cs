using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotator : MonoBehaviour {
	Transform xform;

	[SerializeField] [Range(0f, 1f)] float rotateSpeed = 0f;
	[SerializeField] [Range(0f, 1f)] float tiltAmount = 0f;
	[SerializeField] bool spinMode = true;
	[SerializeField] [Range(0f, 1f)] float tiltIncrease = 1f;
	[SerializeField] [Range(0f, 1f)] float rotateIncrease = 1f;
	[SerializeField] float startDelay = 5f;

	float delayTimer = 0f;
	float offset = 0f;

	void OnEnable () {
		xform = GetComponent<Transform>();
	}

	void FixedUpdate () {
		delayTimer += Time.fixedDeltaTime;

		if (delayTimer > startDelay) {
			offset += rotateSpeed * Time.fixedDeltaTime;

			rotateSpeed += rotateIncrease * Time.fixedDeltaTime;
			tiltAmount += tiltIncrease * Time.fixedDeltaTime * 0.01f;
		}

		var target = new Vector3(
			Mathf.Sin(offset * 0.125f),
			-tiltAmount * 2f,
			Mathf.Cos(offset * 0.125f)
		);

		var spinQuat = Quaternion.LookRotation(target, Vector3.up);
		var tiltQuat = xform.rotation = Quaternion.Euler(new Vector3(
			Mathf.Sin(offset) * -tiltAmount * 45f,
			0f,
			Mathf.Cos(offset) * -tiltAmount * 45f
		));

		xform.rotation = spinMode ? spinQuat : tiltQuat;
	}
}
