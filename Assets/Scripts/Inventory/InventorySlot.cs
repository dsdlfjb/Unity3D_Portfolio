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

    // 아이템을 삭제하는 함수
    public void RemoveItem() => UpdateSlot(new Item(), 0);

    // 슬롯에 있는 아이템의 개수를 조정할 수 있는 함수
    public void AddAmount(int value) => UpdateSlot(_item, _amount += value);

    // 슬롯에 아이템이 드래그 앤 드롭을 할 때 발생되는 함수
    public void UpdateSlot(Item item, int amount)
    {
        // 새롭게 슬롯이 갱신되는 경우
        if (amount <= 0)
            item = new Item();

        OnPreUpdate?.Invoke(this);
        this._item = item;
        this._amount = amount;
        OnPostUpdate?.Invoke(this);
    }
    
    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        // 비어있는 오브젝트이면 장착 시킬 수 있도록
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
