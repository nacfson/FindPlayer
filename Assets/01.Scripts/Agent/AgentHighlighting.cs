using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AgentHighlighting : MonoBehaviour {
    protected List<SkinnedMeshRenderer> _meshRendererList;
    [SerializeField] private Transform _visualTransform;

    [SerializeField] private Material _outlineMaterial;

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

    public void AddMaterial(bool result) {
        Action<bool> action = delegate (bool result) {
            foreach(SkinnedMeshRenderer m in _meshRendererList) {
                if(m.materials.Length > 1) {
                    if (result) {
                        List<Material> materials = m.materials.ToList();
                        materials.Add(_outlineMaterial);
                        m.materials = materials.ToArray();
                    }
                    else {
                        Destroy(m.materials[1]);
                    }
                }
                
            }
        };
        action(result);
    }
}