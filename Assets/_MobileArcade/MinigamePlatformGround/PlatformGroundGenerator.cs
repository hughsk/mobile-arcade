using UnityEngine;

#if UNITY_EDITOR
  using UnityEditor;
#endif

public class PlatformGroundGenerator : MonoBehaviour {
  [SerializeField] PlatformGroundManager manager;
  [SerializeField] float spacing = 0.25f;

  public void Rebuild () {
    if (manager == null) {
      throw new System.Exception("Please make sure you have the manager attached");
    }

    var xform = GetComponent<Transform>();
    var count = xform.childCount;

    while (count++ < 4) {
      var go = new GameObject();

      go.transform.SetParent(xform, false);
      go.name = "Plane (" + count + ")";
    }

    var prim = GameObject.CreatePrimitive(PrimitiveType.Plane);
    var mesh = prim.GetComponent<MeshFilter>().sharedMesh;
    var material = prim.GetComponent<MeshRenderer>().sharedMaterial;

    for (int i = 0; i < 4; i++) {
      var child = xform.GetChild(i).gameObject;
      var filter = GetOrAdd<MeshFilter>(child);
      var renderer = GetOrAdd<MeshRenderer>(child);

      if (filter.sharedMesh == null) filter.sharedMesh = mesh;
      if (renderer.sharedMaterial == null) renderer.sharedMaterial = material;
    }

    var gridWidth = manager.gridSize * (1f + 1f / (float)manager.gridCount) + spacing;

    var left = xform.GetChild(0);
    left.localScale = new Vector3(7.5f, 1f, 20f);
    left.localPosition = -0.5f * new Vector3((left.localScale.x * 10f + gridWidth), 0f, 0f);

    var right = xform.GetChild(1);
    right.localScale = left.localScale;
    right.localPosition = left.localPosition * -1f;

    var top = xform.GetChild(2);
    top.localScale = new Vector3(gridWidth * 0.1f, 1f, 10f);
    top.localPosition = -0.5f * new Vector3(0f, 0f, (top.localScale.z * 10f + gridWidth));

    var bottom = xform.GetChild(3);
    bottom.localScale = top.localScale;
    bottom.localPosition = top.localPosition * -1f;

    DestroyImmediate(prim);
  }

  C GetOrAdd<C> (GameObject go) where C : Component {
    var component = go.GetComponent<C>();
    if (component == null) component = go.AddComponent<C>();
    return component;
  }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PlatformGroundGenerator))]
public class PlatformGroundGeneratorEditor : Editor {
  public override void OnInspectorGUI () {
    base.OnInspectorGUI();
    if (GUILayout.Button("Rebuild")) Rebuild();
  }

  void Rebuild () {
    (this.serializedObject.targetObject as PlatformGroundGenerator).Rebuild();
  }
}
#endif