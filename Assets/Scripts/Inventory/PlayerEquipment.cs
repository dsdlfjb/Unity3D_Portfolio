using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public InventoryObject _equipment;
    public ItemObject[] _defaultItemObjects = new ItemObject[8];     // 기본적으로 장착시킬 아이템

    EquipmentCombiner _combiner;
    ItemInstances[] _itemInstances = new ItemInstances[8];

    private void Awake()
    {
        _combiner = new EquipmentCombiner(gameObject);

        for (int i = 0; i < _equipment.Slots.Length; i++)
        {
            _equipment.Slots[i].OnPreUpdate += OnRemoveItem;
            _equipment.Slots[i].OnPostUpdate += OnEquipItem;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (InventorySlot slot in _equipment.Slots)
        {
            OnEquipItem(slot);
        }
    }

    void OnEquipItem (InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;
        if (itemObject == null)
        {
            EquipDefaultItemBy(slot._allowedItems[0]);
            return;
        }

        int index = (int)slot._allowedItems[0];

        switch (slot._allowedItems[0])
        {
            case EItemType.Helmet:
            case EItemType.Armor:
            case EItemType.Pants:
            case EItemType.Boots:
            case EItemType.Gloves:
                _itemInstances[index] = EquipSkinnedItem(itemObject);
                break;

            case EItemType.Pauldrons:
            case EItemType.LeftWeapon:
            case EItemType.RightWeapon:
                _itemInstances[index] = EquipMeshItem(itemObject);
                break;
        }

        if (_itemInstances[index] != null)
            _itemInstances[index].name = slot._allowedItems[0].ToString();
    }

    ItemInstances EquipSkinnedItem(ItemObject itemObject)
    {
        if (itemObject == null)
            return null;

        Transform itemTransform = _combiner.AddLimb(itemObject._modelPrefab, itemObject._bornNames);

        ItemInstances instances = itemTransform.gameObject.AddComponent<ItemInstances>();

        if (instances != null)
            instances._items.Add(itemTransform);

        return instances;
    }

    ItemInstances EquipMeshItem(ItemObject itemObject)
    {
        if (itemObject == null)
            return null;

        Transform[] itemTransforms = _combiner.AddMesh(itemObject._modelPrefab);

        if (itemTransforms.Length > 0)
        {
            ItemInstances instances = new GameObject().AddComponent<ItemInstances>();
            foreach (Transform t in itemTransforms)
            {
                instances._items.Add(t);
            }

            instances.transform.parent = transform;

            return instances;
        }

        return null;
    }

    void EquipDefaultItemBy(EItemType type)
    {
        int index = (int)type;

        ItemObject itemObject = _defaultItemObjects[index];

        switch(type)
        {
            case EItemType.Helmet:
            case EItemType.Armor:
            case EItemType.Pants:
            case EItemType.Boots:
            case EItemType.Gloves:
                _itemInstances[index] = EquipSkinnedItem(itemObject);
                break;

            case EItemType.Pauldrons:
            case EItemType.LeftWeapon:
            case EItemType.RightWeapon:
                _itemInstances[index] = EquipMeshItem(itemObject);
                break;
        }
    }

    void RemoveItemBy(EItemType type)
    {
        int index = (int)type;

        if (_itemInstances[index] != null)
        {
            Destroy(_itemInstances[index].gameObject);
            _itemInstances[index] = null;
        }
    }

    void OnRemoveItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;

        if (itemObject == null)
        {
            RemoveItemBy(slot._allowedItems[0]);
            return;
        }

        if (slot.ItemObject._modelPrefab != null)
        {
            RemoveItemBy(slot._allowedItems[0]);
        }
    }
}
