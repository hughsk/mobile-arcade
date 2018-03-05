using UnityEngine;

/// <summary>
/// There should only be one prefab containing this MonoBehaviour,
/// a prefab object that should be included in every scene. We can
/// then use this object knowing that it will always be present on
/// any scene, and that it will persist from one scene to the next.
///
/// Instead of writing new functionality in the bootstrapper directly,
/// it's probably better to create a new component and add it to the
/// bootstrap prefab.
/// </summary>
public class Bootstrap : MonoBehaviour {
  static Bootstrap instance;

  public static T Get<T> () {
    return instance.GetComponent<T>();
  }

  void Awake () {
    if (instance != null) {
      Destroy(gameObject);
      return;
    }

    instance = this;
    DontDestroyOnLoad(gameObject);
  }

  void OnDestroy () {
    instance = null;
  }
}
