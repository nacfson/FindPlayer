using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ItemobjectList")]
public class ItemObjectListSO : ScriptableObject{
    public List<ItemObject> objects = new List<ItemObject>();

    public ItemObject GetRandomItem(){
        int r = Random.Range(0,objects.Count);
        return objects[r];
    }
}