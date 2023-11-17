using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory", menuName = "Inventory System/Inventoey")]
public class InventoryObject : ScriptableObject
{
    public ItemDataBase _database;
    public EInterfaceType _eType;

    [SerializeField]
    Inventory _container = new Inventory();

    public Action<ItemObject> OnUseItem;

    public InventorySlot[] Slots => _container._slots;

    public int EmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (InventorySlot slot in Slots)
            {
                if (slot._item._id <= -1)
                    count++;
            }

            return count;
        }
    }

    // ������ �߰� �Լ�
    public bool AddItem(Item item, int amount)
    {
        InventorySlot slot = FindItemInInventory(item);

        if (!_database._itemObjects[item._id]._isStackable || slot == null)
        {
            if (EmptySlotCount <= 0)
                return false;

            GetEmptySlot().UpdateSlot(item, amount);
        }

        else
            slot.AddAmount(amount);

        return true;
    }

    public InventorySlot FindItemInInventory(Item item)
    {
        return Slots.FirstOrDefault(i => i._item._id == item._id);
    }

    // �������� ������ �ִ��� �ƴ��� Ȯ���ϴ� �Լ�
    public bool IsContainItem(ItemObject itemObject)
    {
        return Slots.FirstOrDefault(i => i._item._id == itemObject._data._id) != null;
    }

    public InventorySlot GetEmptySlot()
    {
        return Slots.FirstOrDefault(i => i._item._id <= -1);
    }

    // ���â�� �κ��丮â ���� ���� �������� ��ȯ�ϴ� �Լ�
    public void SwapItems(InventorySlot itemA, InventorySlot itemB)
    {
        if (itemA == itemB) return;

        if (itemB.CanPlaceInSlot(itemA.ItemObject) && itemA.CanPlaceInSlot(itemB.ItemObject))
        {
            InventorySlot tmp = new InventorySlot(itemB._item, itemB._amount);
            itemB.UpdateSlot(itemA._item, itemA._amount);
            itemA.UpdateSlot(tmp._item, tmp._amount);
        }
    }
}
