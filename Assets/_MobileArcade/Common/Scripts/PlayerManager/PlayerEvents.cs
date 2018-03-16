using System;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// Data types used when accepting socket events from socket.io.
/// We need to specify these ahead of time so that we can get
/// nicely typed data from the JSON that socket.io gives us.
/// </summary>
public static class PlayerEvents {
  public struct Input {
    /// <summary>
    /// The player session ID that this event originated from.
    /// </summary>
    public string id;

    /// <summary>
    /// The type of input event, e.g. "tilt"
    /// </summary>
    public string type;

    /// <summary>
    /// A list of input values, which will vary depending on the input.
    /// For example, "tilt" will contain something like [-0.5353, +0.4324].
    /// </summary>
    public float xInput;
    public float yInput;
  }

  public struct Session {
    public string id;
    public int red;
    public int green;
    public int blue;

    public Session (string _id, int red, int green, int blue) {
      this.id = _id;
      this.red = red;
      this.green = green;
      this.blue = blue;
    }

    public static Session FromJSON (string json) {
      return JsonConvert.DeserializeObject<Session>(json);
    }

    public Color color {
      get {
        return new Color(
          (float)red / 255f,
          (float)green / 255f,
          (float)blue / 255f
        );
      }
    }
  }
}