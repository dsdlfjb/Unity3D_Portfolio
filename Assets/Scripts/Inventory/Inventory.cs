using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class Inventory
{
    // �κ��丮 ���� ����
    public InventorySlot[] _slots = new InventorySlot[24];

    // ���Ե��� ��� ����ִ� �Լ�
    public void Clear()
    {
        foreach (InventorySlot slot in _slots)
        {
            slot.UpdateSlot(new Item(), 0);
        }
    }

    // ��� �������� �� �κ��丮�� ������ �Ǿ��ִ��� �ĺ��ϴ� �Լ�
    public bool IsContain(ItemObject itemObject)
    {
        // �κ��丮 ���Կ� �ִ� �������� id�� itemObject�� id�� ���ؼ� null�� �ƴϸ� 
        //������ ������ �������� ���� �Ǿ� �ִٰ� �ĺ��� �� ����
        return IsContain(itemObject._data._id);
        //return Array.Find(_slots, i => i._item._id == itemObject._data._id) != null;
    }

    // id���� ������ �˻� �����ϵ��� �ϴ� �Լ�
    public bool IsContain(int id)
    {
        return _slots.FirstOrDefault(i => i._item._id == id) != null;
    }
}
