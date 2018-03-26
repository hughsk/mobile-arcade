using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class PlatformSingle : MonoBehaviour {
  [SerializeField] Material baseMaterial;
  [SerializeField] Material deadMaterial;

  Transform xform;
  Rigidbody body;
  MeshRenderer render;

  void OnEnable () {
    render = GetComponent<MeshRenderer>();
    xform = GetComponent<Transform>();
    body = GetComponent<Rigidbody>();

    render.sharedMaterial = baseMaterial;
  }

  public void SetPosition (Vector3 pos) {
    xform.localPosition = pos;
  }

  public void SetSize (float scale) {
    xform.localScale = scale * (Vector3.one - Vector3.up * 0.75f);
  }

  public void SetParent (Transform parent) {
    xform.SetParent(parent, false);
  }

  public void Collapse () {
    StartCoroutine(_Collapse());
  }

  IEnumerator _Collapse () {
    render.sharedMaterial = deadMaterial;
    yield return new WaitForSeconds(1f);
    body.isKinematic = false;
    body.detectCollisions = false;
  }
}
