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

  int amountOfPlayers;
  public static int amountOfPlayersAlive;

  PlayerManager playerManager;

  void OnEnable () {

	Physics.gravity = new Vector3(0, -0.5f, 0);
		playerManager = GetComponent<PlayerManager>();

		amountOfPlayers = transform.childCount;
		amountOfPlayersAlive = amountOfPlayers;
		CircularSpawnPoints();
  }

	void CircularSpawnPoints()
	{
		for (int i = 0; i < amountOfPlayers; i++)
		{
			float _x = Mathf.Cos((2*Mathf.PI / amountOfPlayers) * i);
			float _z = Mathf.Sin((2*Mathf.PI / amountOfPlayers) * i);

			transform.GetChild(i).transform.position = centerPoint + new Vector3(_x, 0, _z) * distFromCenter;
		}
	}

	public static void CheckRoundDone()
	{
		if (amountOfPlayersAlive <= 1)
		{
			// Restart level
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
