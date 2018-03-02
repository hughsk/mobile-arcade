using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is just a very simple example player class.
/// It will move slowly in whatever direction the player is tilting.
/// </summary>
public class PlayerExample : Player {
  Transform xform;
  Vector3 direction;
  Rigidbody rb;

  // Acceleration speed of the player
  [SerializeField] float accelerationSpeed;

  // Max velocity of the player
  [SerializeField] float velocityLimit;

  // Bouncing force when two players collide with each other
  [SerializeField] float bouncingForce;

  // 
  // Useful for knowing the velocity before a collision
  Vector3 lastVelocity;

  void OnEnable () {
    xform = GetComponent<Transform>();
	rb = GetComponent<Rigidbody>();
  }

  /*void Update () {
    xform.position += direction * Time.deltaTime;
  }*/

	void FixedUpdate() {
		lastVelocity = rb.velocity;

		rb.AddForce(direction * accelerationSpeed);

		// CLamping the velocity on the X and Z values
		if (Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))  > velocityLimit){
			float y = rb.velocity.y;

			// velocity with only X and Z
			Vector2 _2Dvelocity = new Vector2(rb.velocity.x, rb.velocity.z);
			Vector2 _clamped2DVelocity = Vector2.ClampMagnitude(_2Dvelocity, velocityLimit);

			rb.velocity = new Vector3(_clamped2DVelocity.x, y, _clamped2DVelocity.y);
		}

		/*if (rb.velocity.magnitude > velocityLimit){
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, velocityLimit);
		}*/
	}

  public override void OnMoveTilt(Vector2 movement) {
    direction = new Vector3(movement.x, 0, movement.y);

  }

	void OnCollisionEnter(Collision _col)
	{
		if (_col.gameObject.tag == "Player")
		{
			// Reflection bouncing
			Vector3 _otherVelocity = _col.transform.GetComponent<PlayerExample>().lastVelocity;
			rb.velocity = lastVelocity / 4 + (_otherVelocity / 2)*bouncingForce;
		}
	}
}
