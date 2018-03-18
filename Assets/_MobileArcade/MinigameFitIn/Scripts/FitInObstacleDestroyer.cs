using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitInObstacleDestroyer : MonoBehaviour {

	void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Obstacle")
		{
			// Destroy the whole group of walls at once
			if (coll.transform.parent.GetComponent<FitInObstacle>() != null)
			{
				Destroy(coll.transform.parent.gameObject);
			}

			// If there is no group (meaning this is a wall in itself) Destroy that
			else 
			{
				Destroy(coll.gameObject);
			}
		}
	}
}
