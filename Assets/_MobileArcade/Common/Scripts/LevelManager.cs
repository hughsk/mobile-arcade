using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour {
	[Header("UI")]
	[SerializeField] Transform UICanvasPrefab;
	[SerializeField] GameObject UICountdownPrefab;
	[HideInInspector] public static bool isCountdownOver;
	
	public virtual void OnEnable () {
		Transform UICanvas = Instantiate(UICanvasPrefab);
		Instantiate(UICountdownPrefab, UICanvas);
		isCountdownOver = false;
		Physics.gravity = new Vector3(0, -0.5f, 0);
	}
}
