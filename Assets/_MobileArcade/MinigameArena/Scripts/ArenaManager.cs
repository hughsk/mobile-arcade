using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerManager))]
public class ArenaManager : LevelManager {

  [Header("Player Spawn Points")]
  // Spawn points variables
  [SerializeField] Vector3 centerPoint;
  // Distance from centerpoint
  [SerializeField] float distFromCenter;
	[Space(10)]

	[Header("Barrier")]
	[SerializeField] GameObject barrier;
	[SerializeField] float bDistFromCenter;
	[SerializeField] int amountOfBarriers;

  

	public override void Start () {
		base.Start();

		CircularSpawnPoints();
		CircularBarriers(amountOfBarriers);
  }

	void CircularSpawnPoints()
	{
		//print("Arena Manager Circular Spawn Points");
		players = playerManager.players;
		int count = players.Count;
		int i = 0;

		foreach (var player in players.Values) {
			float _x = Mathf.Cos((2*Mathf.PI / count) * i);
			float _z = Mathf.Sin((2*Mathf.PI / count) * i);

			player.transform.position = centerPoint + new Vector3(_x, 0, _z) * distFromCenter;
			i++;
		}
	}

	void CircularBarriers(int _amountOfBarriers)
	{
		if (barrier == null)
			return;

		for (int i = 0; i < _amountOfBarriers; i++)
		{
			float _x = Mathf.Cos((2*Mathf.PI / _amountOfBarriers) * i);
			float _z = Mathf.Sin((2*Mathf.PI / _amountOfBarriers) * i);

			GameObject _barrier = Instantiate(barrier, centerPoint + new Vector3(_x, 0, _z) * bDistFromCenter, Quaternion.identity);
			_barrier.transform.LookAt(centerPoint);
		}
	}
}
