using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitInObstacleManager : MonoBehaviour {
	[SerializeField] List<GameObject> obstacles;
	[Space(10)]

	[Header("Obstacles Spawn Point")]
	[SerializeField] Vector3 obstacleSpawnPoint;
	bool isSpawning;
	[Space(10)]

	[Header("Obstacle Speed")]
	[SerializeField] float obstacleSpeedStart;
	[SerializeField] float obstacleSpeedAdditioner;
	[SerializeField] float obstacleSpeedMax;
	float obstacleSpeedCurrent;
	[Space(10)]

	[Header("Obstacle Spawn Speed")]
	[SerializeField] float initialSpawnPerSecond;
	[SerializeField] float substactTimePerSpawn;
	[SerializeField] float minSpawnPerSecond;
	float currentSpawnPerSecond;

	// Use this for initialization
	void OnEnable () {
		isSpawning = false;
		obstacleSpeedCurrent = obstacleSpeedStart;
		currentSpawnPerSecond = initialSpawnPerSecond;
	}

	// Update is called once per frame
	void Update () {
		StartCoroutine(SpawnObstacles(currentSpawnPerSecond));
	}

	IEnumerator SpawnObstacles(float waitTime)
	{
		if (isSpawning)
			yield break;

		isSpawning = true;

		int _r = Random.Range(0, obstacles.Count);
		GameObject _obstacle = Instantiate(obstacles[_r], obstacleSpawnPoint, obstacles[_r].transform.rotation);

		int _amountOfObsctacles = _obstacle.transform.childCount;
		int _randomObstacle = Random.Range(0, _amountOfObsctacles);
		Destroy(_obstacle.transform.GetChild(_randomObstacle).gameObject);
		_obstacle.transform.position = obstacleSpawnPoint;
	
		_obstacle.GetComponent<FitInObstacle>().speed = obstacleSpeedCurrent;
		obstacleSpeedCurrent = Mathf.Clamp(obstacleSpeedCurrent + obstacleSpeedAdditioner, obstacleSpeedStart, obstacleSpeedMax);

		currentSpawnPerSecond -= substactTimePerSpawn;
		currentSpawnPerSecond = Mathf.Clamp(currentSpawnPerSecond, minSpawnPerSecond, initialSpawnPerSecond);

		yield return new WaitForSeconds(waitTime);
		isSpawning = false;

	}
}
