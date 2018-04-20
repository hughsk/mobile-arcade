using UnityEngine;
using System;
using System.Collections.Generic;
// using Quobject.SocketIoClientDotNet.Client;
// using Newtonsoft.Json;
using System.Net.Sockets;
using System.Threading;
using StringBuilder = System.Text.StringBuilder;
using Stopwatch = System.Diagnostics.Stopwatch;

/// <summary>
/// Coordinates incoming socket messages from the main server,
/// providing a slightly nicer interface for accepting events
/// as they come through. Made a little gross by the fact that
/// socket.io needs to run on a separate thread.
/// </summary>
public class PlayerConnectionManager : MonoBehaviour {
  [SerializeField] string socketHost = "localhost";
  [SerializeField] int socketPort = 3001;
  [SerializeField] bool socketsEnabled = false;

  Dictionary<string, PlayerEvents.Session> sessions = new Dictionary<string, PlayerEvents.Session>();

  public event Action<PlayerEvents.Input> EVENT_INPUT;
  public event Action<PlayerEvents.Session> EVENT_ENTER;
  public event Action<PlayerEvents.Session> EVENT_LEAVE;

  List<PlayerEvents.Input> queueInput = new List<PlayerEvents.Input>();
  List<PlayerEvents.Session> queueEnter = new List<PlayerEvents.Session>();
  List<PlayerEvents.Session> queueLeave = new List<PlayerEvents.Session>();
  object queueLock = new System.Object();

  Thread connectionThread;

  void Awake () {
    DontDestroyOnLoad(gameObject);
  }

  void OnEnable () {
    if (socketsEnabled) OpenSocketConnection();
  }

  TcpClient client;
  NetworkStream stream;
  StringBuilder buffer;
  static byte[] keepalive = new byte[] { (byte)'p', (byte)'i', (byte)'n', (byte)'g', (byte)'\n' };
  bool running = false;

  void ThreadLoop () {
    if (running) return;

    Debug.Log("Attempting connection to central server");

    if (client == null) client = new TcpClient();

    var result = client.BeginConnect(socketHost, socketPort, null, null);

    result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5));

    if (client.Connected) {
      Debug.Log("Connection established!");
      client.EndConnect(result);

      if (stream == null) stream = client.GetStream();
      if (buffer == null) buffer = new StringBuilder();

      OnStartConnection();

      var stopwatch = new Stopwatch();
      running = true;
      stopwatch.Start();

      while (running) {
        while (stream.DataAvailable) {
          var data = (char)stream.ReadByte();
          if (data == '\n') {
            HandleMessage(buffer.ToString());
            buffer.Remove(0, buffer.Length);
          } else {
            buffer.Append(data);
          }
        }

        // Detect disconnection because c# is bad at networking
        if (stopwatch.ElapsedMilliseconds > 1000) {
          try {
            stream.Write(keepalive, 0, keepalive.Length);
          } catch {
            running = false;
            OnEndConnection();
          }

          stopwatch.Reset();
          stopwatch.Start();
        }
      }

      Debug.Log("Server disconnected");
    } else {
      Debug.Log("Connection failed, trying again");
    }

    CloseSocketConnection();
    ThreadLoop();
  }

  char[] messageDelimeter = new char[] { ':' };

  void HandleMessage (string message) {
    if ((message = message.Trim()).Length <= 0) return;

    try {
      var parts = message.Split(messageDelimeter, 2, StringSplitOptions.None);
      if (parts.Length < 1) return;

      var name = parts[0];
      var data = parts[1];

      switch (name) {
        case "ci": OnPlayerInput(data); break;
        case "client-connect": OnPlayerConnect(data); break;
        case "client-disconnect": OnPlayerDisconnect(data); break;
      }

    } catch (System.Exception error) {
      Debug.Log(error.Message);
    }
  }

  void OnDisable () {
    CloseSocketConnection();

    if (connectionThread != null) connectionThread.Abort();
    connectionThread = null;
  }

  void OpenSocketConnection () {
    if (connectionThread != null) {
      connectionThread.Abort();
      connectionThread = null;
    }

    connectionThread = new Thread(ThreadLoop);
    connectionThread.Start();
  }

  void CloseSocketConnection () {
    running = false;
    if (client == null) return;
    client.Close();
    client = null;
    if (stream == null) return;
    stream.Close();
    stream = null;
  }

  void OnStartConnection () {
    // socket.Emit("server:populate", new string[1] { "" });
  }

  void OnEndConnection () {
    foreach (var session in sessions.Values) {
      OnPlayerDisconnect(session.id);
    }
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

  static char[] nullChar = new char[] { '~' };

  void OnPlayerInput (string data) {
    try {
      var message = data.Split(nullChar, 4, StringSplitOptions.None);
      var id = message[0];
      var type = message[1];
      var x = float.Parse(message[2]);
      var y = float.Parse(message[3]);

      lock (queueLock) {
        queueInput.Add(new PlayerEvents.Input {
          id = id,
          type = type,
          xInput = x,
          yInput = y,
        });
      }
    } catch (System.Exception error) {
      Debug.Log(error.Message);
    }
  }

  // Because OnPlayerInput etc. are called on a different thread, we want to make
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
