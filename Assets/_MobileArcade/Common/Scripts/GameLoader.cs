using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {
	bool isLoading = false;
	int startScene = 0;

	void Awake () {
		startScene = SceneManager.GetActiveScene().buildIndex;
		Debug.Log(startScene);
	}

	void Start () {
		LoadNextScene();
	}

	int GetRandomScene () {
		return Mathf.FloorToInt(
			Random.Range(1, SceneManager.sceneCountInBuildSettings)
		);
	}

	public void FinishMinigame () {
		LoadNextScene();
	}

	void LoadNextScene () {
		if (isLoading) return;
		isLoading = true;

		int id = startScene == 0 ? GetRandomScene() : startScene;
		Debug.Log(id);
		StartCoroutine(_LoadNextScene(id));
	}

	IEnumerator _LoadNextScene (int id) {
		var loading = SceneManager.LoadSceneAsync(id);
		while (!loading.isDone) yield return null;
		isLoading = false;
	}
}
