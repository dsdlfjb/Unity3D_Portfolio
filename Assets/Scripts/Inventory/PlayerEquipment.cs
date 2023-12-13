using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public InventoryObject _equipment;
    public ItemObject[] _defaultItemObjects = new ItemObject[9];     // �⺻������ ������ų ������

    EquipmentCombiner _combiner;
    ItemInstances[] _itemInstances = new ItemInstances[9];

    public GameObject _currentWeaponInHand;

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
        // ���â�� �ִ� ���Ե��� �����ͼ� ĳ������ �⺻���� �͵��� ����
        foreach (InventorySlot slot in _equipment.Slots)
        {
            OnEquipItem(slot);
        }
    }

    // ���Կ� �������� ������ �� �������� ���ٸ� �⺻ �������� �������ְ�,
    // �������� �ִٸ� SkinnedMesh �Ǵ� StaticMesh�� �����ϴ� �Լ�
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
            case EItemType.Chest:
            case EItemType.Pants:
            case EItemType.Boots:
            case EItemType.Gloves:
            case EItemType.Belt:
            case EItemType.Pauldrons:
                _itemInstances[index] = EquipSkinnedItem(itemObject);
                break;

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

    // �⺻ ��� �������� �������ִ� �Լ�
    void EquipDefaultItemBy(EItemType type)
    {
        int index = (int)type;

        ItemObject itemObject = _defaultItemObjects[index];

        switch(type)
        {
            case EItemType.Helmet:
            case EItemType.Chest:
            case EItemType.Pants:
            case EItemType.Boots:
            case EItemType.Gloves:
                _itemInstances[index] = EquipSkinnedItem(itemObject);
                break;

            case EItemType.Belt:
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