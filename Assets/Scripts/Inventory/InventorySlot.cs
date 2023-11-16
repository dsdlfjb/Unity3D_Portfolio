using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image _icon;
    public Text _itemName;
    public Text _itemCount;
    public GameObject _selectedItem;

    public void AddItem(Item item)
    {
        _itemName.text = item._itemName;
        _icon.sprite = item._itemIcon;

        if (Item.EItemType.Use == item._eItemType)
        {
            if (item._itemCount > 0)
                _itemCount.text = "x " + item._itemCount.ToString();

            else
                _itemCount.text = "";
        }
    }

    public void RemoveItem()
    {
        _itemName.text = "";
        _itemCount.text = "";
        _icon.sprite = null;
    }
}
