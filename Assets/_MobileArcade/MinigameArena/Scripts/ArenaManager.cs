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
		AssignPlayersColor();

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

	void AssignPlayersColor()
	{
		for (int i = 0; i < amountOfPlayers; i++)
		{
			HSBColor _hsbColor = new HSBColor((1 / (float) amountOfPlayers) * i, 1, 1, 1);
			Color _color = HSBColor.ToColor(_hsbColor);
			transform.GetChild(i).GetComponent<MeshRenderer>().material.color = _color;
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
