using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is just a very simple example player class.
/// It will move slowly in whatever direction the player is tilting.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class FitInPlayer : Player {
  Vector3 direction;
  Rigidbody rb;
  // Useful for knowing the velocity before a collision
  Vector3 lastVelocity;
  
  [Header("Player Movement")]
  // Acceleration speed of the player
  [SerializeField] float accelerationSpeed;
  // Max velocity of the player
  [SerializeField] float velocityLimit;
  // Bouncing force when two players collide with each other
  [SerializeField] float bouncingForce;
  [Space(10)]


  [Header("Particle Systems")]
  // Star particles (used when two players collide)
  [SerializeField] ParticleSystem starParticles;
  float startParticles_lifetime;
  [Space(10)]


  [Header("Audio Clips")]
  [SerializeField] List<AudioClip> collidingSounds;
  bool canPlay;



  void OnEnable () {
	rb = GetComponent<Rigidbody>();
  }

	void Start () {
		/*float angle = Random.Range(0f, Mathf.PI * 2f);
		float _x = Mathf.Cos(angle);
		float _z = Mathf.Sin(angle);

		xform.position = centerPoint + new Vector3(_x, 0, _z) * dist;

		HSBColor _hsbColor = new HSBColor(Random.Range(0f, 1f), 1, 1, 1);
		Color _color = HSBColor.ToColor(_hsbColor);
		GetComponent<MeshRenderer>().material.color = _color;*/
		startParticles_lifetime = starParticles.main.startLifetime.constant;
	}
		

	void FixedUpdate() {
		lastVelocity = rb.velocity;
		canPlay = true;

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

  public override void OnMoveTilt(Vector2 _movement) {
		direction = new Vector3(_movement.x, 0, _movement.y);

  }

	void OnCollisionEnter(Collision _col)
	{
		if (_col.gameObject.tag == "Player")
		{
			// Reflection bouncing
			Vector3 _otherVelocity = _col.transform.GetComponent<FitInPlayer>().lastVelocity;
			rb.velocity = lastVelocity / 4 + (_otherVelocity / 2)*bouncingForce;

			// Star Particles effect
			ParticleSystem _starParticles = Instantiate(
				starParticles, 
				_col.contacts[0].point, 
				Quaternion.LookRotation(_col.contacts[0].normal)
			);
			Destroy(_starParticles.gameObject, startParticles_lifetime);

			// Play a random "Booing" sound
			if (canPlay)
			{
				// Makes sure two sounds won't play at the same time
				_col.collider.GetComponent<FitInPlayer>().canPlay = false;
	
				// Make new GameObject for a sound
				int _r = Random.Range(0, collidingSounds.Count);
				GameObject _soundObj = new GameObject("Booing Sound " + _r);

				// Position of sound at collision point 
				_soundObj.transform.position = _col.contacts[0].point;

				AudioSource _soundObj_audioSource = _soundObj.AddComponent<AudioSource>();
				//_soundObj_audioSource.pitch = Map(lastVelocity.magnitude, 0f, velocityLimit, 0.8f, 1.2f, true);
				_soundObj_audioSource.GetComponent<AudioSource>().PlayOneShot(collidingSounds[_r]);
				Destroy(_soundObj, collidingSounds[_r].length);

			}

		}
	}

	/*float Map(float x, float in_min, float in_max, float out_min, float out_max, bool shouldClamp)
	{
		float result =  (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
		if (shouldClamp)
		{
			result = Mathf.Clamp(result, out_min, out_max);
		}
		return result;
	}*/
}
