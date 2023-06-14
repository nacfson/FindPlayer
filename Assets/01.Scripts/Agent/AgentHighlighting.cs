using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AgentHighlighting : MonoBehaviour {
    protected List<SkinnedMeshRenderer> _meshRendererList;
    [SerializeField] private Transform _visualTransform;

    private void Awake() {
        _meshRendererList = new List<SkinnedMeshRenderer>();
        _visualTransform.GetComponentsInChildren<SkinnedMeshRenderer>(_meshRendererList);
    }

    public void SetMaterial(float value,Color color) {
        foreach(var m in _meshRendererList) {
            Material[] mats = m.materials;
            mats[mats.Length - 1].SetFloat("_LineThickness", value);
            mats[mats.Length - 1].SetColor("_OutlineColor", color);
            m.materials = mats;
        }
    }

    public void SetMaterialOpacity(float value) {
        Action<float> action = delegate (float value) {
            foreach(SkinnedMeshRenderer m in _meshRendererList) {
                MaterialPropertyBlock mp = new MaterialPropertyBlock();
                m.GetPropertyBlock(mp);
                mp.SetFloat("_LineOpacity", value);
                m.SetPropertyBlock(mp);
            }
        };
        action(value);
    }


}