using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MovementData")]
public class MovementData : ScriptableObject{
    public float Speed;
    public float RunSpeed;

    public float JumpHeight;
}