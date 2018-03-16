using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerManager))]
public class FitInManager : MonoBehaviour {

  [Header("Player Spawn Points")]
  // Spawn points variables
  [SerializeField] Vector3 centerPoint;
  // Distance from centerpoint
  [SerializeField] float distBetweenPlayers;
  // How many players should be aligned in a row, until a new row appears infront
  [SerializeField] int amountOfPlayersOnRow;

  int amountOfPlayers;
  public static int amountOfPlayersAlive;

  PlayerManager playerManager;

  void OnEnable () {

	Physics.gravity = new Vector3(0, -0.5f, 0);
	playerManager = GetComponent<PlayerManager>();

	amountOfPlayers = transform.childCount;
	amountOfPlayersAlive = amountOfPlayers;
	EquidistantSpawnPoints();
  }

	void EquidistantSpawnPoints()
	{
		// Makes sure it stays centered
		if (amountOfPlayersOnRow > amountOfPlayers)
		{
			amountOfPlayersOnRow = amountOfPlayers;
		}

		int _j = 0;

		for (int i = 0; i < amountOfPlayers; i++)
		{
			if (_j >= amountOfPlayersOnRow)
			{
				_j = 0;
			}
			float _x = ((float)-amountOfPlayersOnRow/2 + _j)*distBetweenPlayers + distBetweenPlayers/2 + centerPoint.x;
			float _y = centerPoint.y;
			float _z = centerPoint.z + (i/amountOfPlayersOnRow)*distBetweenPlayers; // If there is a row of 4 players, make another row in front
			transform.GetChild(i).transform.position = new Vector3(_x, _y, _z);
			_j++;
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
