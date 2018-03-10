using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]
public class ArenaManager : MonoBehaviour {

  [Header("Player Spawn Points")]
  // Spawn points variables
  [SerializeField] Vector3 centerPoint;
  // Distance from centerpoint
  [SerializeField] float distFromCenter;

  int amountOfPlayers;

  Transform xform;
  PlayerManager playerManager;

  void OnEnable () {
	Physics.gravity = new Vector3(0, -0.5f, 0);
    xform = GetComponent<Transform>();
		playerManager = GetComponent<PlayerManager>();

		amountOfPlayers = transform.childCount;
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
}
