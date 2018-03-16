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
      SpawnNewPlayer("0", new Color(1, 0, 0));
      SpawnNewPlayer("1", new Color(0, 0, 1));
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

  Player SpawnNewPlayer (string sessionId, Color color) {
    if (players.ContainsKey(sessionId)) return players[sessionId];

    var player = Instantiate<Player>(playerPrefab);
    var playerTransform = player.transform;

    player.SetColor(color);
    playerTransform.SetParent(xform);
    players[sessionId] = player;

    Debug.Log(sessionId);
    Debug.Log(player);

    return player;
  }

  void OnPlayerEnter (PlayerEvents.Session session) {
    SpawnNewPlayer(session.id, session.color);
  }

  void OnPlayerInput (PlayerEvents.Input input) {
    Player player;
    if (!players.TryGetValue(input.id, out player)) return;
    if (player == null) return;
    if (input.type == "tilt") {
      player.OnMoveTilt(new Vector2(
        input.xInput,
        input.yInput
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
