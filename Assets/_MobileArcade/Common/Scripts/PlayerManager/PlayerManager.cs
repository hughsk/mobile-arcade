using UnityEngine;
using System.Collections.Generic;
using Quobject.SocketIoClientDotNet.Client;

/// <summary>
/// Accepts incoming player connections, disconnections and input events from
/// PlayerConnectionManager. Spawns a player prefab every time one connects,
/// and destroys it when it disconnects.
/// </summary>
public class PlayerManager : MonoBehaviour {
  [SerializeField] Player playerPrefab;

  [Header("Debug Options")]
  [SerializeField] bool testPlayers = true;

  PlayerConnectionManager connectionManager;

  Dictionary<string, Player> players;

  Transform xform;

  void OnEnable () {
    xform = GetComponent<Transform>();
    players = new Dictionary<string, Player>();

    connectionManager = Bootstrap.Get<PlayerConnectionManager>();
    connectionManager.EVENT_ENTER += OnPlayerEnter;
    connectionManager.EVENT_LEAVE += OnPlayerLeave;
    connectionManager.EVENT_INPUT += OnPlayerInput;
    connectionManager.Join();

    if (testPlayers) {
      SpawnNewPlayer("0");
      SpawnNewPlayer("1");
    }
  }

  void OnDisable () {
    foreach (var player in players.Values) {
      if (player != null) Destroy(player.gameObject);
    }

    players.Clear();
    connectionManager.EVENT_ENTER -= OnPlayerEnter;
    connectionManager.EVENT_LEAVE -= OnPlayerLeave;
    connectionManager.EVENT_INPUT -= OnPlayerInput;
  }

  void OnValidate () {
    if (playerPrefab == null) {
      throw new System.Exception("Must supply a valid player prefab");
    }
  }

  void Update () {
    if (testPlayers) {
      //players["0"].OnMoveTilt(Utils.KeyboardVector("w", "a", "s", "d"));
      players["1"].OnMoveTilt(Utils.KeyboardVector("z", "q", "s", "d"));
      players["0"].OnMoveTilt(Utils.KeyboardVector("i", "j", "k", "l"));
    }
  }

  Player SpawnNewPlayer (string sessionId) {
    if (players.ContainsKey(sessionId)) return players[sessionId];

    var player = Instantiate<Player>(playerPrefab);
    var playerTransform = player.transform;

    playerTransform.SetParent(xform);
    players.Add(sessionId, player);

    return player;
  }

  void OnPlayerEnter (PlayerEvents.Session session) {
    SpawnNewPlayer(session.id);
  }

  void OnPlayerInput (PlayerEvents.Input input) {
    if (players[input.id] == null) return;
    if (input.type == "tilt") {
      players[input.id].OnMoveTilt(new Vector2(
        input.inputs[0],
        input.inputs[1]
      ));
    }
  }

  void OnPlayerLeave (PlayerEvents.Session session) {
    Player player;

    if (players.TryGetValue(session.id, out player)) {
      Destroy(player.gameObject);
    }
  }
}
