using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelManager : MonoBehaviour {
	[Header("UI")]
	[SerializeField] Transform UICanvasPrefab;
	[SerializeField] GameObject UICountdownPrefab;
	[HideInInspector] public static bool isCountdownOver;
	[Space(10)]

	protected PlayerManager playerManager;
	[HideInInspector] public Dictionary<string, Player> players;
	
	public virtual void OnEnable () {
		Transform UICanvas = Instantiate(UICanvasPrefab);
		Instantiate(UICountdownPrefab, UICanvas);
		isCountdownOver = false;
		Physics.gravity = new Vector3(0, -0.5f, 0);

		playerManager = GetComponent<PlayerManager>();
	}
}
