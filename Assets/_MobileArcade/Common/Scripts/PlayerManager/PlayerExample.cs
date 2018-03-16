using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is just a very simple example player class.
/// It will move slowly in whatever direction the player is tilting.
/// </summary>
public class PlayerExample : Player {
  Transform xform;
  Vector3 direction;

  void OnEnable () {
    xform = GetComponent<Transform>();
  }

  void Update () {
    xform.position += direction * Time.deltaTime;
  }

  public override void OnMoveTilt(Vector2 _movement) {
    direction = new Vector3(_movement.x, 0, _movement.y);
  }

  public override void SetColor(Color color) {
    Debug.Log(color);
  }
}
