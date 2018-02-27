using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {
  [SerializeField] Player playerPrefab;

  [Header("Debug Options")]
  [SerializeField] bool useWithoutPhone;

  List<Player> players;
  Transform xform;

  void OnEnable () {
    xform = GetComponent<Transform>();

    players = new List<Player>();

    if (useWithoutPhone) {
      SpawnNewPlayer();
      SpawnNewPlayer();
    }
  }

  void OnDisable () {
    for (int i = 0; i < players.Count; i++) {
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
      players[0].OnMoveTilt(Utils.KeyboardVector("w", "a", "s", "d"));
      players[1].OnMoveTilt(Utils.KeyboardVector("i", "j", "k", "l"));
    }
  }

  Player SpawnNewPlayer () {
    var player = Instantiate<Player>(playerPrefab);
    var playerTransform = player.transform;

    playerTransform.SetParent(xform);
    players.Add(player);

    return player;
  }
}
