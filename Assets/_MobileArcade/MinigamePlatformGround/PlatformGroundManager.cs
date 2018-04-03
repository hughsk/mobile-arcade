using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGroundManager : MonoBehaviour {
	[SerializeField] [Range(2, 12)] public int gridCount = 8;
	[SerializeField] [Range(0, 15)] public float gridSize = 1f;
	[SerializeField] Material gridMaterial;
	[SerializeField] Mesh gridCellMesh;
	[SerializeField] PhysicMaterial gridPhysicsMaterial;

	List<Rigidbody> boxes = new List<Rigidbody>();
	Transform xform;

	void OnEnable () {
		xform = GetComponent<Transform>();
		if (Application.isPlaying) Populate();
	}

	float timer = 0f;
	float dropPeriod = 3f;

	void Update () {
		timer += Time.deltaTime;

		while (timer > dropPeriod) {
			timer -= dropPeriod;
			DropOneBox();
		}
	}

	void DropOneBox () {
		var idx = Random.Range(0, boxes.Count);
		var box = boxes[idx];

		box.isKinematic = false;
		boxes.RemoveAt(idx);
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
				primTransform.localScale = Vector3.one * GetCellScale() * 0.9f;
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
			0f,
			GetCellPosition(y)
		);
	}
}
