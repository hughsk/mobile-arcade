using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerManager))]
public class ArenaManager : MonoBehaviour {

  [Header("Player Spawn Points")]
  // Spawn points variables
  [SerializeField] Vector3 centerPoint;
  // Distance from centerpoint
  [SerializeField] float distFromCenter;

  PlayerManager playerManager;

  void OnEnable () {

	Physics.gravity = new Vector3(0, -0.5f, 0);
		playerManager = GetComponent<PlayerManager>();

		CircularSpawnPoints();
  }

	void CircularSpawnPoints()
	{
		var players = playerManager.players;
		int count = players.Count;
		int i = 0;

		foreach (var player in players.Values) {
			float _x = Mathf.Cos((2*Mathf.PI / count) * i);
			float _z = Mathf.Sin((2*Mathf.PI / count) * i);

			player.transform.position = centerPoint + new Vector3(_x, 0, _z) * distFromCenter;
			i++;
		}
	}
}
