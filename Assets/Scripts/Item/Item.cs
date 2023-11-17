using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int _id = -1;
    public string _name;

    public ItemBuff[] _buffs;

    public Item()
    {
        _id = -1;
        _name = "";
    }

    public Item (ItemObject itemObject)
    {
        _name = itemObject.name;
        _id = itemObject._data._id;

        _buffs = new ItemBuff[itemObject._data._buffs.Length];

        for (int i =0; i <_buffs.Length; i++)
        {
            _buffs[i] = new ItemBuff(itemObject._data._buffs[i].Min, itemObject._data._buffs[i].Max)
            {
                _stats = itemObject._data._buffs[i]._stats
            };
        }
    }
}