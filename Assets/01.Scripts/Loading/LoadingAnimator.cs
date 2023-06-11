using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimator : AgentAnimator{
    protected override void Awake() {
        _animator = GetComponent<Animator>();
        _actionData = transform.parent.Find("AD").GetComponent<ActionData>();
    }
}