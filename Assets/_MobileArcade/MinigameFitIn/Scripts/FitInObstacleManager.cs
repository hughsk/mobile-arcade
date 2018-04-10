using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitInObstacleManager : MonoBehaviour {
	[SerializeField] List<GameObject> obstacles;

	[Space(10)]
	[SerializeField] List<GameObject> uniqueObstacles;
	[SerializeField] float uniqueObtaclesSpawnDelay;
	[SerializeField] int spawnProbability; // 1 / X

	[Space(10)]

	[Header("Obstacles Spawn Point")]
	[SerializeField] Vector3 obstacleSpawnPoint;
	bool isSpawning;
	[Space(10)]

	[Header("Obstacle Speed")]
	[SerializeField] float obstacleSpeedStart;
	[SerializeField] float obstacleSpeedAdditioner;
	[SerializeField] float obstacleSpeedMax;
	[HideInInspector] public float obstacleSpeedCurrent;
	[Space(10)]

	[Header("Obstacle Spawn Speed")]
	[SerializeField] float initialSpawnPerSecond;
	[SerializeField] float substactTimePerSpawn;
	[SerializeField] float minSpawnPerSecond;
	[SerializeField] float uniqueObstacleDelay;
	float currentSpawnPerSecond;

	float currentTime;


	// Use this for initialization
	void OnEnable () {
		isSpawning = false;
		obstacleSpeedCurrent = obstacleSpeedStart;
		currentSpawnPerSecond = initialSpawnPerSecond;
	}

	// Update is called once per frame
	void Update () {
		if (!LevelManager.isCountdownOver) return;

		currentTime += Time.deltaTime;

		if (currentTime > uniqueObtaclesSpawnDelay)
		{
			int _r = Random.Range(0, spawnProbability);

			if (_r < 1)
			{
				StartCoroutine(SpawnUniqueObstacles(currentSpawnPerSecond + 1));
				return;
			}
		}


		StartCoroutine(SpawnObstacles(currentSpawnPerSecond));
	}

	IEnumerator SpawnObstacles(float waitTime)
	{
		if (isSpawning)
			yield break;

		isSpawning = true;



		int _r = Random.Range(0, obstacles.Count);
		if ( Random.Range(0, 2) == 1)
			_r = 0;

		GameObject _obstacle = Instantiate(obstacles[_r], obstacleSpawnPoint, obstacles[_r].transform.rotation);

		int _amountOfObstaclesToDestroy = Random.Range(1, _obstacle.transform.childCount);
		int _amountOfObstaclesDestroyed = 0;

		List<int> _childsDestroyed = new List<int>();

		while (_amountOfObstaclesDestroyed < _amountOfObstaclesToDestroy)
		{
			int _randomObstacleNr = Random.Range(0, _obstacle.transform.childCount);
			if (_childsDestroyed.Contains(_randomObstacleNr))
				continue;
			
			Destroy(_obstacle.transform.GetChild(_randomObstacleNr).gameObject);
			_childsDestroyed.Add(_randomObstacleNr);
			_amountOfObstaclesDestroyed++;
		}
		_obstacle.transform.position = obstacleSpawnPoint;
	
		_obstacle.GetComponent<FitInObstacle>().speed = obstacleSpeedCurrent;
		obstacleSpeedCurrent = Mathf.Clamp(obstacleSpeedCurrent + obstacleSpeedAdditioner, obstacleSpeedStart, obstacleSpeedMax);

		currentSpawnPerSecond -= substactTimePerSpawn;
		currentSpawnPerSecond = Mathf.Clamp(currentSpawnPerSecond, minSpawnPerSecond, initialSpawnPerSecond);

		yield return new WaitForSeconds(waitTime);
		isSpawning = false;

	}

	IEnumerator SpawnUniqueObstacles(float waitTime)
	{
		if (isSpawning)
			yield break;

		isSpawning = true;

		int _r = Random.Range(0, uniqueObstacles.Count);
		GameObject _uniqueObstacle = Instantiate(uniqueObstacles[_r], obstacleSpawnPoint, uniqueObstacles[_r].transform.rotation);

		_uniqueObstacle.transform.position = obstacleSpawnPoint;

		_uniqueObstacle.GetComponent<FitInObstacle>().speed = obstacleSpeedCurrent;
		obstacleSpeedCurrent = Mathf.Clamp(obstacleSpeedCurrent + obstacleSpeedAdditioner, obstacleSpeedStart, obstacleSpeedMax);

		currentSpawnPerSecond -= substactTimePerSpawn;
		currentSpawnPerSecond = Mathf.Clamp(currentSpawnPerSecond, minSpawnPerSecond, initialSpawnPerSecond);

		yield return new WaitForSeconds(waitTime);
		isSpawning = false;
	}
}
