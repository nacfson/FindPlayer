using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AgentHighlighting : MonoBehaviour {
    protected List<SkinnedMeshRenderer> _meshRendererList;
    [SerializeField] private Transform _visualTransform;

    private void Awake() {
        _meshRendererList = new List<SkinnedMeshRenderer>();
        _visualTransform.GetComponentsInChildren<SkinnedMeshRenderer>(_meshRendererList);
        //Debug.LogError(_meshRendererList.Count);
    }

    public void SetMaterial(float value) {
        foreach(var m in _meshRendererList) {
            Material[] mats = m.materials;
            mats[mats.Length - 1].SetFloat("_LineThickness", value);
            m.materials = mats;
        }
    }
}