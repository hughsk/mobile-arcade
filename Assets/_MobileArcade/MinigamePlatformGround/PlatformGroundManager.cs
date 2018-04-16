using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGroundManager : MonoBehaviour {
	[SerializeField] [Range(2, 12)] public int gridCount = 8;
	[SerializeField] [Range(0, 15)] public float gridSize = 1f;
	[SerializeField] [Range(0, 1)] public float cellHeight = 0.25f;
	[SerializeField] Material gridMaterial;
	[SerializeField] Mesh gridCellMesh;
	[SerializeField] PhysicMaterial gridPhysicsMaterial;
	[SerializeField] Color fallColor;

	List<Rigidbody> boxes = new List<Rigidbody>();
	Transform xform;

	void OnEnable () {
		xform = GetComponent<Transform>();
		if (Application.isPlaying) Populate();
	}

	float dropPeriodMin = 2f;
	float dropPeriodMax = 4f;
	float timer = 5f;
	float speedupCounter = 0f;
	float speedupRate = 1.015f;

	void Update () {
		timer -= Time.deltaTime;

		if (timer <= 0) {
			float increment = Random.Range(dropPeriodMin, dropPeriodMax);
			increment = increment / Mathf.Pow(speedupRate, speedupCounter);
			increment = Mathf.Max(0.25f, increment);

			if (float.IsNaN(increment)) increment = 0.25f;

			timer += increment;
			DropOneBox();
		}
	}

	void DropOneBox () {
		if (boxes.Count <= 0) return;

		var idx = Random.Range(0, boxes.Count);
		var box = boxes[idx];

		speedupCounter++;
		boxes.RemoveAt(idx);
		box.GetComponent<MeshRenderer>().material.color = fallColor;
		StartCoroutine(BeginDrop(box));
	}

	IEnumerator BeginDrop (Rigidbody box) {
		yield return new WaitForSeconds(3f);
		box.isKinematic = false;
	}

	void Populate () {
		boxes.Clear();

		for (int x = 0; x < gridCount; x++) {
			for (int y = 0; y < gridCount; y++) {
				var prim = new GameObject();
				var primTransform = prim.transform;
				var primBody = prim.AddComponent<Rigidbody>();

				primBody.isKinematic = true;
				boxes.Add(primBody);

				prim.AddComponent<MeshFilter>().sharedMesh = gridCellMesh;
				prim.AddComponent<MeshRenderer>().sharedMaterial = gridMaterial;
				prim.AddComponent<BoxCollider>().sharedMaterial = gridPhysicsMaterial;
				primTransform.SetParent(xform, false);
				primTransform.localScale = new Vector3(1, cellHeight, 1) * GetCellScale() * 0.9f;
				primTransform.localPosition = GetCellPosition3(x, y);
			}
		}
	}

	void OnDrawGizmosSelected () {
		Utils.GizmoMatrix(transform.localToWorldMatrix, () => {
			var scaleDir = (Vector3.right + Vector3.forward);

			Gizmos.color = Color.green;
			for (int x = 0; x < gridCount; x++) {
				for (int y = 0; y < gridCount; y++) {
					Gizmos.DrawWireCube(GetCellPosition3(x, y), scaleDir * GetCellScale());
				}
			}
		});
	}

	float GetCellScale () {
		return gridSize / (float)(gridCount - 1);
	}

	float GetCellPosition (int x) {
		return gridSize * (((float)x / (float)(gridCount - 1)) - 0.5f);
	}

	Vector2 GetCellPosition (int x, int y) {
		return new Vector2(GetCellPosition(x), GetCellPosition(y));
	}

	Vector3 GetCellPosition3 (int x, int y) {
		return new Vector3(
			GetCellPosition(x),
			GetCellScale() * (1 - cellHeight) * 0.9f * 0.5f,
			GetCellPosition(y)
		);
	}
}
