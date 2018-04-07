using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Quobject.SocketIoClientDotNet.Client;
using System.Collections;

/// <summary>
/// Accepts incoming player connections, disconnections and input events from
/// PlayerConnectionManager. Spawns a player prefab every time one connects,
/// and destroys it when it disconnects.
/// </summary>
public class PlayerManager : MonoBehaviour {

  [SerializeField] Player playerPrefab;

  [Header("Debug Options")]
  [SerializeField] bool testPlayers = true;
  [SerializeField] bool WASDControls = true;

  PlayerConnectionManager connectionManager;

  [HideInInspector] public Dictionary<string, Player> players;
  [HideInInspector] public HashSet<string> playersThatHaveDied;

  Transform xform;
  bool soloPlay = true;

  void OnEnable () {
    xform = GetComponent<Transform>();
    players = new Dictionary<string, Player>();
    playersThatHaveDied = new HashSet<string>();

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
      if (WASDControls) {
        players["0"].OnMoveTilt(Utils.KeyboardVector("w", "a", "s", "d"));
      } else {
        players["0"].OnMoveTilt(Utils.KeyboardVector("z", "q", "s", "d"));
      }

      players["1"].OnMoveTilt(Utils.KeyboardVector("i", "j", "k", "l"));
    }

    var activePlayers = players.Count;
    var remainingPlayers = activePlayers - playersThatHaveDied.Count;

    // The game is in "solo" mode until at least 2 players join:
    // when the game is being played solo, the player can move freely
    // and the game is only restarted when the player dies.
    soloPlay = soloPlay && (activePlayers < 2);

    if (activePlayers > 0 && remainingPlayers <= (soloPlay ? 0 : 1)) {
			Bootstrap.Get<GameLoader>().FinishMinigame();
    }
  }

  Player SpawnNewPlayer (string sessionId, Color color) {
    if (players.ContainsKey(sessionId)) return players[sessionId];

    var player = Instantiate<Player>(playerPrefab);
    var playerTransform = player.transform;

    player.sessionId = sessionId;
    player.SetColor(color);
    playerTransform.SetParent(xform);
    players[sessionId] = player;

    Debug.Log(sessionId);

    return player;
  }

  public void MakePlayerLose (Player player) {
    Destroy(player.gameObject);
    playersThatHaveDied.Add(player.sessionId);
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

    Debug.Log("removing " + session.id);
    playersThatHaveDied.Remove(session.id);

    if (players.TryGetValue(session.id, out player)) {
      try {
        var gObj = player.gameObject;
        if (gObj != null && gObj.activeInHierarchy) Destroy(player.gameObject);
      } catch (MissingReferenceException) {}

      players.Remove(session.id);
    }
  }
		
}
