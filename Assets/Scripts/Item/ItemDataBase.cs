using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item Database", menuName ="Inventory System/Items/Database")]
public class ItemDataBase : ScriptableObject
{
    public ItemObject[] _itemObjects;

    public void OnValidate()
    {
        for (int i = 0; i < _itemObjects.Length; i++)
        {
            _itemObjects[i]._data._id = i;
        }
    }
}
