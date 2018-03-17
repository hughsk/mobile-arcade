using UnityEngine;
using System;
using System.Collections.Generic;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

/// <summary>
/// Coordinates incoming socket messages from the main server,
/// providing a slightly nicer interface for accepting events
/// as they come through. Made a little gross by the fact that
/// socket.io needs to run on a separate thread.
/// </summary>
public class PlayerConnectionManager : MonoBehaviour {
  [SerializeField] string socketHost = "http://localhost:3000/";
  [SerializeField] bool socketsEnabled = false;

  Socket socket;

  Dictionary<string, PlayerEvents.Session> sessions = new Dictionary<string, PlayerEvents.Session>();

  public event Action<PlayerEvents.Input> EVENT_INPUT;
  public event Action<PlayerEvents.Session> EVENT_ENTER;
  public event Action<PlayerEvents.Session> EVENT_LEAVE;

  List<PlayerEvents.Input> queueInput = new List<PlayerEvents.Input>();
  List<PlayerEvents.Session> queueEnter = new List<PlayerEvents.Session>();
  List<PlayerEvents.Session> queueLeave = new List<PlayerEvents.Session>();
  object queueLock = new System.Object();

  void Awake () {
    DontDestroyOnLoad(gameObject);
  }

  void OnEnable () {
    if (socketsEnabled) OpenSocketConnection();
  }

  void OnDisable () {
    CloseSocketConnection();
  }

  void OpenSocketConnection () {
    if (socket != null) return;

    socket = IO.Socket(socketHost);
    socket.On(Socket.EVENT_CONNECT, OnSocketConnection);
    socket.On(Socket.EVENT_DISCONNECT, OnSocketDisconnect);
    socket.On("client:connect", OnPlayerConnect);
    socket.On("client:disconnect", OnPlayerDisconnect);
    socket.On("client:input", OnClientInput);
  }

  void CloseSocketConnection () {
    if (socket == null) return;

    Debug.Log("Forcing socket connection to close");
    socket.Disconnect();
    socket = null;
  }

  void OnSocketConnection () {
    Debug.LogWarning("Successfully connected to main socket server!");
  }

  void OnSocketDisconnect () {
    Debug.LogError("Socket connection terminated?");
  }

  void OnPlayerConnect (object id) {
    var session = PlayerEvents.Session.FromJSON(id.ToString());
    sessions.Add(session.id, session);
    lock (queueLock) {
      queueEnter.Add(session);
    }
  }

  void OnPlayerDisconnect (object _id) {
    var id = _id.ToString();

    PlayerEvents.Session session;
    if (!sessions.TryGetValue(id, out session)) return;

    sessions.Remove(id.ToString());
    lock (queueLock) {
      queueLeave.Add(session);
    }
  }

  void OnClientInput (object data) {
    var input = JsonConvert.DeserializeObject<PlayerEvents.Input>(data.ToString());
    lock (queueLock) {
      queueInput.Add(input);
    }
  }

  // Because OnClientInput etc. are called on a different thread, we want to make
  // sure that any events we call are triggered from the main thread. This gives
  // us full access to Unity when we need it.
  void Update () {
    lock (queueLock) {
      if (EVENT_ENTER != null) for (int i = 0; i < queueEnter.Count; i++) EVENT_ENTER(queueEnter[i]);
      if (EVENT_INPUT != null) for (int i = 0; i < queueInput.Count; i++) EVENT_INPUT(queueInput[i]);
      if (EVENT_LEAVE != null) for (int i = 0; i < queueLeave.Count; i++) EVENT_LEAVE(queueLeave[i]);
      queueEnter.Clear();
      queueInput.Clear();
      queueLeave.Clear();
    }
  }

  /// <summary>
  /// Call this once when you've loaded up a new minigame.
  /// It will load up any players who joined before the game
  /// started in the same way that any future players joining will.
  /// </summary>
  public void Join () {
    if (EVENT_ENTER != null) {
      foreach (var session in sessions.Values) {
        EVENT_ENTER(session);
      }
    }
  }
}