using UnityEngine;

public static partial class Utils {
  /// <summary>
  /// Takes a series of WASD-like keys and returns a vector
  /// with the direction they should move in. Useful as a
  /// quick way to provide multiple keyboard inputs for testing.
  /// </summary>
  public static void GizmoMatrix (Matrix4x4 currMatrix, System.Action GizmoMatrixAction) {
    var prevMatrix = Gizmos.matrix;
    Gizmos.matrix = currMatrix;
    GizmoMatrixAction();
    Gizmos.matrix = prevMatrix;
  }
}