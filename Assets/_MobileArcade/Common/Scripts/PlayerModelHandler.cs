using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerModelHandler : MonoBehaviour {
	Transform xform;
	Animator animator;

	[SerializeField] Rigidbody body;
	[SerializeField] bool rolling = false;
	[SerializeField] SkinnedMeshRenderer meshRenderer;

	void OnEnable () {
		xform = GetComponent<Transform>();
		animator = GetComponent<Animator>();
	}

	void LateUpdate () {
		if (!rolling) {
			var horizontalVelocity = new Vector3(body.velocity.x, 0f, body.velocity.z);
			animator.speed = horizontalVelocity.magnitude * 3.5f;
			xform.rotation = Quaternion.LookRotation(horizontalVelocity, Vector3.up);
		}
	}

	public void UpdateColor (Color color) {
		meshRenderer.materials[0].color = color;
	}
}
