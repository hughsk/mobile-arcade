using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {
  [SerializeField] Player playerPrefab;

  [Header("Debug Options")]
  [SerializeField] bool useWithoutPhone;
  [SerializeField] Vector3 centerPoint;
  [SerializeField] float dist;

  List<Player> players;
  Transform xform;

  void OnEnable () {
	Physics.gravity = new Vector3(0, -0.5f, 0);

    xform = GetComponent<Transform>();

    players = new List<Player>();

    if (useWithoutPhone) {
      SpawnNewPlayer();
      SpawnNewPlayer();  
    }

	CircularSpawnPoints();
	AssignPlayersColor();
  }

  void OnDisable () {
    for (int i = 0; i < players.Count; i++) {
	  if (players[i] == null)
	  	continue;
			
      Destroy(players[i].gameObject);
    }

    players.Clear();
  }

  void OnValidate () {
    if (playerPrefab == null) {
      throw new System.Exception("Must supply a valid player prefab");
    }
  }

  void Update () {
    if (useWithoutPhone) {
      //players[1].OnMoveTilt(Utils.KeyboardVector("w", "a", "s", "d"));
	  players[1].OnMoveTilt(Utils.KeyboardVector("z", "q", "s", "d"));
      players[0].OnMoveTilt(Utils.KeyboardVector("i", "j", "k", "l"));
    }
  }

  Player SpawnNewPlayer () {
    var player = Instantiate<Player>(playerPrefab);
    var playerTransform = player.transform;

    playerTransform.SetParent(xform);
    players.Add(player);

    return player;
  }

	void CircularSpawnPoints()
	{
		for (int i = 0; i < players.Count; i++)
		{
			float _x = Mathf.Cos((2*Mathf.PI / players.Count) * i);
			float _z = Mathf.Sin((2*Mathf.PI / players.Count) * i);

			players[i].transform.position = centerPoint + new Vector3(_x, 0, _z) * dist;
		}
	}

	void AssignPlayersColor()
	{
		for (int i = 0; i < players.Count; i++)
		{
			HSBColor _hsbColor = new HSBColor((1 / (float) players.Count) * i, 1, 1, 1);
			Color _color = HSBColor.ToColor(_hsbColor);
			players[i].GetComponent<MeshRenderer>().material.color = _color;
		}
	}
}
