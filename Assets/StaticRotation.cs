using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticRotation : MonoBehaviour {

	Transform xform;
	Transform parent;

	// Use this for initialization
	void OnEnable () {
		xform = GetComponent<Transform>();
		parent = xform.parent;
	}

	// Update is called once per frame
	void Update () {
		xform.rotation = Quaternion.identity;
		xform.position = parent.position + Vector3.up * 0.1f;
	}
}
