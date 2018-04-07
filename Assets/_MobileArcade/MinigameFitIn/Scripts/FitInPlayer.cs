using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is just a very simple example player class.
/// It will move slowly in whatever direction the player is tilting.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class FitInPlayer : BallPlayer {
  // Makes sure you don't get your velocity mirrored twice when touching 2 obstacles at once
  bool touchedObstacle;


	public override void ClampingSpeed()
	{
		if (!touchedObstacle)
		{
			base.ClampingSpeed();
		}
	}
  
	public override void OnCollisionEnter(Collision _col)
	{
		base.OnCollisionEnter(_col);

		if (_col.gameObject.tag == "Obstacle" && !touchedObstacle)
		{
			rb.velocity = -1 * lastVelocity;
			StartCoroutine(AllowVelocityIncrease(5));
		}

	}

	IEnumerator AllowVelocityIncrease(float waitTime)
	{
		touchedObstacle = true;
		yield return new WaitForSeconds(waitTime);
		touchedObstacle = false;
	}
}
