using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerManager))]
public class FitInManager : LevelManager {

  [Header("Player Spawn Points")]
  // Spawn points variables
  [SerializeField] Vector3 centerPoint;
  // Distance from centerpoint
  [SerializeField] float distBetweenPlayers;
  // How many players should be aligned in a row, until a new row appears infront
  [SerializeField] int amountOfPlayersOnRow;

	public override void OnEnable () {
	base.OnEnable();

	EquidistantSpawnPoints();
  }

	void EquidistantSpawnPoints()
	{
		players = playerManager.players;
		int count = players.Count;

		// Makes sure it stays centered
		if (amountOfPlayersOnRow > count)
		{
			amountOfPlayersOnRow = count;
		}

		int _j = 0;

		for (int i = 0; i < count; i++)
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
}
