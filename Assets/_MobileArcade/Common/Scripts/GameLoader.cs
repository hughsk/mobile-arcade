using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {
	bool isLoading = false;
	int startScene = 0;

	[SerializeField] AudioClip music;
	AudioSource source;

	void Awake () {
		startScene = SceneManager.GetActiveScene().buildIndex;
	}

	void Start () {
		LoadNextScene();
	}

	void OnEnable () {
		if (source == null && GetComponent<AudioSource>() == null) {
			source = gameObject.AddComponent<AudioSource>();
			source.clip = music;
			source.loop = true;
			source.volume = 0.25f;
			source.Play();
			Debug.Log("Starting music");
		}
	}

	int GetRandomScene () {
		int currScene = SceneManager.GetActiveScene().buildIndex;
		int nextScene = Mathf.Max(1, currScene);

		if (SceneManager.sceneCountInBuildSettings > 2) {
			while (currScene == nextScene) {
				nextScene = Mathf.FloorToInt(
					Random.Range(1, SceneManager.sceneCountInBuildSettings)
				);
			}
		}

		Debug.Log(nextScene);

		return nextScene;
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
