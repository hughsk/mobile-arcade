using System;

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
    public Session (string _id) {
      this.id = _id;
    }
  }
}