using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentHighlighting : MonoBehaviour {
    [SerializeField] protected SkinnedMeshRenderer _meshRenderer;
    protected Material _material;
    private void Awake() {
        _material = _meshRenderer.material;
        Debug.Log(_material);
    }

    public Material GetMaterial() {
        return _material;
    }
    
    public void SetMaterial(Material material) {
        _meshRenderer.material = material;
    }


}