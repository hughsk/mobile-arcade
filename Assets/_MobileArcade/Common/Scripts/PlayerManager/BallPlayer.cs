using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPlayer : Player {
	Vector3 direction;
	protected Rigidbody rb;
	// Useful for knowing the velocity before a collision
	protected Vector3 lastVelocity;

	[Header("Player Movement")]
	// Acceleration speed of the player
	[SerializeField] float accelerationSpeed;
	// Max velocity of the player
	[SerializeField] float velocityLimit;
	// Bouncing force when two players collide with each other
	[SerializeField] float bouncingForce;
	[Space(10)]

	[Header("Arrow")]
	// The mesh used for the arrow that points out from the player
	[SerializeField] Mesh arrowMesh;
	// The material used for the arrow
	[SerializeField] Material arrowMaterial;
	[SerializeField, Range(0f, 1f)] float arrowUpOffset = 0.1f;
	[SerializeField, Range(0f, 1f)] float arrowOutOffset = 0.1f;
	[SerializeField, Range(0f, 3f)] float arrowScale = 1.5f;
	[Space(10)]


	[Header("Particle Systems")]
	// Star particles (used when two players collide)
	[SerializeField] ParticleSystem starParticles;
	float startParticles_lifetime;
	// Dust trail particles (used when the player moves)
	[SerializeField] ParticleSystem dustTrails; // The prefab
	[Space(10)]


	[Header("Audio Clips")]
	[SerializeField] List<AudioClip> collidingSounds;
	[SerializeField] AudioClip fallingWhistle;
	GameObject whistleSoundObj;
	AudioSource whistleSoundAudiosource;
	// Useful so players won't instantiate multiple sounds at once during a collision
	bool canPlay;

	Transform xform;
	Matrix4x4 arrowMatrix = Matrix4x4.identity;

	// Used for the collision animation
	Animator anim;

	// Use this for initialization
	public virtual void OnEnable ()
	{
		xform = GetComponent<Transform>();
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();

		startParticles_lifetime = starParticles.main.startLifetime.constant;

		// Dust Trails
		if (dustTrails != null)
		{
			ParticleSystem _playerDustTrails= Instantiate(dustTrails, transform.position, Quaternion.identity);
			_playerDustTrails.gameObject.name = "Dust Trail Effect";
			_playerDustTrails.transform.SetParent(transform, true);
			_playerDustTrails.transform.localScale = dustTrails.transform.localScale;
		}

		else {
			throw new System.Exception("Dust Trail particle is not assigned to Player prefab");
		}
	}

	public virtual void Update () {
		if (!LevelManager.isCountdownOver) return;

		if (xform == null) return;

		var normal = direction.normalized;
		var origin = xform.position + Vector3.up * arrowUpOffset + normal * arrowOutOffset;
		var matrix = Matrix4x4.LookAt(origin, origin + normal * 10f, Vector3.up);

		arrowMatrix = matrix * Matrix4x4.Scale(Vector3.one * arrowScale);

		Graphics.DrawMesh(arrowMesh, arrowMatrix, arrowMaterial, LayerMask.NameToLayer("Default"), Camera.main);


		// Whistle sound if player is falling
		if (rb.velocity.y < -0.2 && whistleSoundObj == null)
		{
			print(rb.velocity.y);
			whistleSoundObj = new GameObject("Whistle Sound");
			whistleSoundObj.transform.SetParent(transform);
			whistleSoundObj.transform.position = transform.position;

			whistleSoundAudiosource = whistleSoundObj.AddComponent<AudioSource>();
			whistleSoundAudiosource.PlayOneShot(fallingWhistle);
			Destroy(whistleSoundObj,fallingWhistle.length);
		}

		// Lower the volume repetitively
		else if (whistleSoundObj != null)
		{
			whistleSoundAudiosource.volume-= 0.007f;

			if (rb.velocity.y >= 0)
			{
				Destroy(whistleSoundObj);
			}
		}

	}

	public virtual void FixedUpdate() {
		if (!LevelManager.isCountdownOver) return;

		lastVelocity = rb.velocity;
		canPlay = true;

		// Animation
		anim.SetBool("isColliding", false);

		rb.AddForce(direction * accelerationSpeed);

		ClampingSpeed();
	}

	public virtual void ClampingSpeed()
	{
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

	public override void SetColor (Color color)
	{
		var mesh = GetComponent<MeshRenderer>();
		if (mesh != null) {
			mesh.material.color = color;
			return;
		}

		var model = GetComponentInChildren<PlayerModelHandler>();
		if (model != null) model.UpdateColor(color);
	}

	public override void OnMoveTilt(Vector2 _movement) {
		direction = new Vector3(_movement.x, 0, _movement.y);
	}

	public virtual void OnCollisionEnter(Collision _col)
	{
		if (_col.gameObject.tag == "Player")
		{
			print("ball collision");
			// Reflection bouncing
			Vector3 _otherVelocity = _col.transform.GetComponent<BallPlayer>().lastVelocity;
			rb.velocity = lastVelocity / 4 + (_otherVelocity / 2)*bouncingForce;

		}

		if (_col.gameObject.tag == "Player" ||
			_col.gameObject.tag == "Barrier" ||
			_col.gameObject.tag == "Obstacle")
		{
			// Collision animation
			anim.SetBool("isColliding", true);

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
				ArenaPlayer _arenaPlayer = _col.collider.GetComponent<ArenaPlayer>();
				if (_arenaPlayer != null)
					_col.collider.GetComponent<ArenaPlayer>().canPlay = false;

				// Make new GameObject for a sound
				int _r = Random.Range(0, collidingSounds.Count);
				GameObject _soundObj = new GameObject("Booing Sound " + _r);

				// Position of sound at collision point
				_soundObj.transform.position = _col.contacts[0].point;

				AudioSource _soundObj_audioSource = _soundObj.AddComponent<AudioSource>();
				//_soundObj_audioSource.pitch = Map(lastVelocity.magnitude, 0f, velocityLimit, 0.8f, 1.2f, true);
				_soundObj_audioSource.PlayOneShot(collidingSounds[_r]);
				Destroy(_soundObj, collidingSounds[_r].length);
			}
		}
	}
}
