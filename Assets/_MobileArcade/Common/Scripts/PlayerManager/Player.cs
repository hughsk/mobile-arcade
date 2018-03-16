using UnityEngine;
using System;

/// <summary>
/// The Player class is a base class for you to inherit from.
///
/// You can find a super simple example in the PlayerExample class.
/// </summary>
public abstract class Player : MonoBehaviour {
  /// <summary>
  /// Another class will call OnMoveTilt when the current player is supposed
  /// to move in a particular direction. It's then up to you to move the player
  /// however you please :)
  /// </summary>
  /// <param name="movement">
  /// An almost-normalized (x, y) vector pointing in the direction the player
  /// should move. May have a magnitude smaller than 1 if the player is only
  /// moving slowly.
  /// </param>
  public abstract void SetColor (Color color);
  public virtual void OnMoveTilt (Vector2 movement) {}
}