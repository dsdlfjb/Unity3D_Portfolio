using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Inventory System/Items/New Item")]
public class ItemObject : ScriptableObject
{
    public EItemType _eItemType;
    public bool _isStackable;

    public Sprite _icon;
    public GameObject _modelPrefab;     // 캐릭터에게 장착될 프리팹

    public Item _data = new Item();

    // 캐릭터 조합 시스템
    public List<string> _bornNames = new List<string>();

    [TextArea(15, 20)]
    public string _description;

    // 데이터를 변경하면 호출되는 함수
    private void OnValidate()
    {
        _bornNames.Clear();

        if (_modelPrefab == null || _modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>() == null)
            return;

        SkinnedMeshRenderer renderer = _modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
        Transform[] bones = renderer.bones;

        foreach (Transform t in bones)
        {
            _bornNames.Add(t.name);
        }
    }

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}