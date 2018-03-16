using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitInWallManager : MonoBehaviour {
	[SerializeField] List<GameObject> walls;
	[Space(10)]

	[Header("Walls Spawn Point")]
	[SerializeField] Vector3 wallSpawnPoint;
	bool isSpawning;
	[Space(10)]

	[Header("Wall Speed")]
	[SerializeField] float wallSpeedStart;
	[SerializeField] float wallSpeedMultiplier;
	[SerializeField] float wallSpeedMax;
	float wallSpeedCurrent;

	// Use this for initialization
	void OnEnable () {
		isSpawning = false;
		wallSpeedCurrent = wallSpeedStart;
	}

	// Update is called once per frame
	void Update () {
		StartCoroutine(SpawnWalls(5));
	}

	IEnumerator SpawnWalls(float waitTime)
	{
		if (isSpawning)
			yield break;

		isSpawning = true;

		int _r = Random.Range(0, walls.Count);
		GameObject _wall = Instantiate(walls[_r], wallSpawnPoint, walls[_r].transform.rotation);
		_wall.transform.position = wallSpawnPoint;
		_wall.GetComponent<FitInWall>().speed = wallSpeedCurrent;
		wallSpeedCurrent = Mathf.Clamp(wallSpeedCurrent * wallSpeedMultiplier, wallSpeedStart, wallSpeedMax);

		yield return new WaitForSeconds(waitTime);
		isSpawning = false;

	}
}
