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
  [SerializeField] float forceSpeed;
  [SerializeField] float velocityLimit;

  void OnEnable () {
    xform = GetComponent<Transform>();
	rb = GetComponent<Rigidbody>();
  }

  /*void Update () {
    xform.position += direction * Time.deltaTime;
  }*/

	void FixedUpdate() {
		rb.AddForce(direction * forceSpeed);

		if (rb.velocity.magnitude > velocityLimit){
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, velocityLimit);
		}
	}

  public override void OnMoveTilt(Vector2 movement) {
    direction = new Vector3(movement.x, 0, movement.y);

  }
}
