using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitInObstacle : MonoBehaviour {
	/*[Header("Spawn Point")]
	[SerializeField] Vector3 spawnPoint;
	[Space(10)]*/

	[Header("obstacles Movement")]

	// Speed at which the obstacle is moving
	[HideInInspector] public float speed;

	// Rigidbodies of 1 or more obstacles
	Rigidbody[] rbs;

	// Use this for initialization
	void OnEnable () {
		// Check if this gameObject is a parent of obstacles, acting as a folder/empty GameObject
		if (GetComponent<Renderer>() == null &&
			transform.childCount > 0)
		{
			rbs = GetComponentsInChildren<Rigidbody>();
			foreach(Rigidbody rb in rbs)
			{
				rb.isKinematic = true;
				rb.useGravity = false;
				rb.gameObject.tag = "Obstacle";
			}
		}

		// Else it's a obstacle in itself.
		else 
		{
			gameObject.tag = "Obstacle";
			rbs[0] = GetComponent<Rigidbody>();
		}
	}
		
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 newPos = new Vector3(0, 0, speed * Time.fixedDeltaTime);

		foreach(Rigidbody rb in rbs)
		{
			if (rb != null)
				rb.MovePosition(rb.transform.position - newPos);
		}
	}
}
