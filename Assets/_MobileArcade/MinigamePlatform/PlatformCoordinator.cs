using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCoordinator : MonoBehaviour {
	[SerializeField] [Range(1, 10)] int size = 4;
	[SerializeField] float boxSize = 0.1f;
	[SerializeField] PlatformSingle boxObject;

	List<PlatformSingle> platforms = new List<PlatformSingle>();
	Transform xform;

	[SerializeField]
	float dropFrequency = 5f;
	float dropCountdown = 0f;

	void OnEnable () {
		xform = GetComponent<Transform>();
		dropCountdown = dropFrequency;

		EachPlatformPosition((Vector3 pos) => {
			var platform = Instantiate<PlatformSingle>(boxObject);

			platform.SetPosition(pos);
			platform.SetSize(boxSize);
			platform.SetParent(xform);
			platforms.Add(platform);
		});
	}

	void Update () {
		dropCountdown -= Time.deltaTime;

		while (platforms.Count > 0 && dropCountdown < 0f) {
			dropCountdown += dropFrequency;

			int platformIndex = Mathf.FloorToInt(UnityEngine.Random.Range(0, platforms.Count));

			Debug.Log(platformIndex);
			platforms[platformIndex].Collapse();
			platforms.RemoveAt(platformIndex);
		}
	}

	void OnDisable () {
		for (int i = 0; i < platforms.Count; i++) {
			Destroy(platforms[i].gameObject);
		}
	}

	void OnDrawGizmos () {
		var matrix = Gizmos.matrix;
		Gizmos.matrix = transform.localToWorldMatrix;

		EachPlatformPosition((Vector3 pos) => {
			Gizmos.DrawWireCube(pos, boxSize * (Vector3.one - Vector3.up * 0.75f));
		});

		Gizmos.matrix = matrix;
	}

	void EachPlatformPosition (Action<Vector3> Callback) {
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				float xx = boxSize * (x - size / 2f + 0.5f);
				float yy = boxSize * (y - size / 2f + 0.5f);
				Callback(new Vector3(xx, 0, yy));
			}
		}
	}
}
