using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InventorySlot
{
    public EItemType[] _allowedItems = new EItemType[0];

    [NonSerialized]
    public InventoryObject _parent;
    [NonSerialized]
    public GameObject _slotUI;

    [NonSerialized]
    public Action<InventorySlot> OnPreUpdate;
    [NonSerialized]
    public Action<InventorySlot> OnPostUpdate;

    public Item _item;
    public int _amount;

    public ItemObject ItemObject
    {
        get
        {
            return _item._id >= 0 ? _parent._database._itemObjects[_item._id] : null;
        }
    }

    public InventorySlot() => UpdateSlot(new Item(), 0);
    public InventorySlot(Item item, int amount) => UpdateSlot(item, amount);

    // �������� �����ϴ� �Լ�
    public void RemoveItem() => UpdateSlot(new Item(), 0);

    // ���Կ� �ִ� �������� ������ ������ �� �ִ� �Լ�
    public void AddAmount(int value) => UpdateSlot(_item, _amount += value);

    // ���Կ� �������� �巡�� �� ����� �� �� �߻��Ǵ� �Լ�
    public void UpdateSlot(Item item, int amount)
    {
        // ���Ӱ� ������ ���ŵǴ� ���
        if (amount <= 0)
            item = new Item();

        OnPreUpdate?.Invoke(this);
        this._item = item;
        this._amount = amount;
        OnPostUpdate?.Invoke(this);
    }
    
    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        // ����ִ� ������Ʈ�̸� ���� ��ų �� �ֵ���
        if (_allowedItems.Length <= 0 || itemObject == null || itemObject._data._id < 0)
            return true;

        foreach (EItemType eItemType in _allowedItems)
        {
            if (itemObject._eItemType == eItemType)
                return true;
        }

        return false;
    }
}
