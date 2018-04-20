using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitInBackground : MonoBehaviour {
	[SerializeField] FitInObstacleManager fitInObstacleManager;
	[SerializeField] Transform background1;
	[SerializeField] Transform background2;
	Vector3 background1Pos;
	Vector3 background2Pos;

	// Use this for initialization
	void Start () {
		background1Pos = background1.transform.position;
		background2Pos = background2.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		CheckSecondBackground();
	}

	void FixedUpdate()
	{
		var speed = Time.fixedDeltaTime * fitInObstacleManager.obstacleSpeedCurrent;
		background1.position += new Vector3(0, 0, -speed);
		background2.position += new Vector3(0, 0, -speed);
	}

	void CheckSecondBackground()
	{
		if (background2.transform.position.z <= background1Pos.z)
		{
			background1.transform.position = background1Pos;
			background2.transform.position = background2Pos;
		}
	}


}
