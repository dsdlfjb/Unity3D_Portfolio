using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItems : MonoBehaviour
{
    public InventoryObject _equipmentObject;
    public InventoryObject _inventoryObject;
    public ItemDataBase _databaseObject;

    public void AddNewItem()
    {
        if (_databaseObject._itemObjects.Length > 0)
        {
            ItemObject newItemObject = _databaseObject._itemObjects[Random.Range(0, _databaseObject._itemObjects.Length - 1)];
            //ItemObject newItemObject = _databaseObject._itemObjects[_databaseObject._itemObjects.Length - 1];
            Item newItem = new Item(newItemObject);

            _inventoryObject.AddItem(newItem, 1);
        }
    }

    public void ClearInventory()
    {
        _equipmentObject?.Clear();
        _inventoryObject?.Clear();
    }
}
