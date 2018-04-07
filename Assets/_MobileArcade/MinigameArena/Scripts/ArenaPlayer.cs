using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is just a very simple example player class.
/// It will move slowly in whatever direction the player is tilting.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ArenaPlayer : BallPlayer {
  

	// Makes sure you don't get your velocity mirrored twice when touching 2 barriers at once
	bool touchedBarrier;


	public override void OnCollisionEnter(Collision _col)
	{
		base.OnCollisionEnter(_col);


		if (_col.gameObject.tag == "Barrier" && !touchedBarrier)
		{
			// Mirror bouncing
			rb.velocity = -lastVelocity;
			// Makes sure you don't get your velocity mirrored twice when touching 2 barriers at once
			StartCoroutine(DontMirrorVelocity(0.1f));
			Destroy(_col.gameObject);
		}
			
	}


	// Makes sure you don't get your velocity mirrored twice when touching 2 barriers at once
	IEnumerator DontMirrorVelocity(float waitTime)
	{
		touchedBarrier = true;
		yield return new WaitForSeconds(waitTime);
		touchedBarrier = false;
	}
		
}
