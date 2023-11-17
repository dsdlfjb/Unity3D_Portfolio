using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class Inventory
{
    // 인벤토리 슬롯 생성
    public InventorySlot[] _slots = new InventorySlot[24];

    // 슬롯들을 모두 비워주는 함수
    public void Clear()
    {
        foreach (InventorySlot slot in _slots)
        {
            slot.UpdateSlot(new Item(), 0);
        }
    }

    // 어떠한 아이템이 이 인벤토리에 포함이 되어있는지 식별하는 함수
    public bool IsContain(ItemObject itemObject)
    {
        // 인벤토리 슬롯에 있는 아이템의 id와 itemObject의 id를 비교해서 null이 아니면 
        //동일한 종류의 아이템이 포함 되어 있다고 식별할 수 있음
        return IsContain(itemObject._data._id);
        //return Array.Find(_slots, i => i._item._id == itemObject._data._id) != null;
    }

    // id만을 가지고 검색 가능하도록 하는 함수
    public bool IsContain(int id)
    {
        return _slots.FirstOrDefault(i => i._item._id == id) != null;
    }
}
