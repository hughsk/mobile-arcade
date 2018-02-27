using UnityEngine;

public static partial class Utils {
  /// <summary>
  /// Takes a series of WASD-like keys and returns a vector
  /// with the direction they should move in. Useful as a
  /// quick way to provide multiple keyboard inputs for testing.
  /// </summary>
  public static Vector2 KeyboardVector (
    string forwards,
    string left,
    string backwards,
    string right
  ) {
    float x = 0f;
    float y = 0f;

    if (Input.GetKey(forwards)) y += 1f;
    if (Input.GetKey(backwards)) y -= 1f;
    if (Input.GetKey(right)) x += 1f;
    if (Input.GetKey(left)) x -= 1f;

    return new Vector2(x, y).normalized;
  }
}