using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECharacterAttribute
{
    Agility,
    Intellect,
    Stamina,
    Strength
}

[System.Serializable]
public class ItemBuff : MonoBehaviour
{
    public ECharacterAttribute _eState;
    public int _value;

    [SerializeField] int _min;
    [SerializeField] int _max;

    public int Min => _min;
    public int Max => _max;

    public ItemBuff(int min, int max)
    {
        this._min = _min;
        this._max = _max;

        GenerateValue();
    }

    public void GenerateValue()
    {
        _value = Random.Range(_min, _max);
    }

    public void AddValue(ref int v)
    {
        v += _value;
    }
}
