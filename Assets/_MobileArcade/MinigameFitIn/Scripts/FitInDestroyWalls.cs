using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitInDestroyWalls : MonoBehaviour {

	void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Wall")
		{
			// Destroy the whole group of walls at once
			if (coll.transform.parent.GetComponent<FitInWall>() != null)
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
