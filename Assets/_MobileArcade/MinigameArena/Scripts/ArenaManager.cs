using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]
public class ArenaManager : MonoBehaviour {
  // Spawn points variables
  [SerializeField] Vector3 centerPoint;
  // Distance from centerpoint
  [SerializeField] float dist;

  Transform xform;
  PlayerManager playerManager;

  void OnEnable () {
	Physics.gravity = new Vector3(0, -0.5f, 0);
    xform = GetComponent<Transform>();
		playerManager = GetComponent<PlayerManager>();
  }

  void Start () {
    CircularSpawnPoints();
    AssignPlayersColor();
  }

	void CircularSpawnPoints()
	{
		var players = playerManager.players;

		for (int i = 0; i < players.Count; i++)
		{
			float _x = Mathf.Cos((2*Mathf.PI / players.Count) * i);
			float _z = Mathf.Sin((2*Mathf.PI / players.Count) * i);

			players[i].transform.position = centerPoint + new Vector3(_x, 0, _z) * dist;
		}
	}

	void AssignPlayersColor()
	{
		var players = playerManager.players;

		for (int i = 0; i < players.Count; i++)
		{
			HSBColor _hsbColor = new HSBColor((1 / (float) players.Count) * i, 1, 1, 1);
			Color _color = HSBColor.ToColor(_hsbColor);
			players[i].GetComponent<MeshRenderer>().material.color = _color;
		}
	}
}
