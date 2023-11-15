using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int _itemID;
    public string _itemName;
    public string _itemDescription;
    public int _itemCount;
    public Sprite _itemIcon;
    public EItemType _eItemType;

    public enum EItemType
    {
        Use,
        Equip,
        Quest,
        Etc
    }

    public Item(int itemID, string itemName, string itemDescription,  EItemType eItemType,  int itemCount = 1)
    {
        _itemID = itemID;
        _itemName = itemName;
        _itemDescription = itemDescription;
        _itemCount = itemCount;
        _eItemType = eItemType;
        _itemIcon = Resources.Load("Item/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
    }
}
