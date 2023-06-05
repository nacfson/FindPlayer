using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ProvocationList")]
public class ProvocationData : ScriptableObject{
    public List<string> provocations = new List<string>();

    public string RandomProvocation(){
        int random = Random.Range(0,provocations.Count );
        return provocations[random];
    }
}