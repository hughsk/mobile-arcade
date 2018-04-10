using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitInBackground : MonoBehaviour {
	[SerializeField] FitInObstacleManager fitInObstacleManager;
	[SerializeField] Transform environmentObject;
	[SerializeField] float repeatInterval = 50;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate()
	{
		var speed = Time.fixedDeltaTime * fitInObstacleManager.obstacleSpeedCurrent;
		environmentObject.position += new Vector3(0, 0, -speed);
	}


}
