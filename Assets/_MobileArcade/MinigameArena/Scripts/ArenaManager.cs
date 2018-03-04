using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]
public class ArenaManager : MonoBehaviour {
  // Spawn points variables
  [SerializeField] Vector3 centerPoint;
  // Distance from centerpoint
  [SerializeField] float dist;

  Transform xform;
  PlayerManager playerManager;

  void OnEnable () {
	Physics.gravity = new Vector3(0, -0.5f, 0);
    xform = GetComponent<Transform>();
		playerManager = GetComponent<PlayerManager>();
  }
}
