using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInventoryUI : InventoryUI
{
    [SerializeField] protected GameObject _slotPrefab;
    [SerializeField] protected Vector2 _start;
    [SerializeField] protected Vector2 _size;
    [SerializeField] protected Vector2 _space;

    [Min(1), SerializeField] protected int _numberOfColumn = 4;

    private void Start()
    {
        gameObject.SetActive(false);    
    }

    public override void CreateSlots()
    {
        _slotUIs = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < _inventoryObject.Slots.Length; i++)
        {
            GameObject go = Instantiate(_slotPrefab, Vector3.zero, Quaternion.identity, transform);
            go.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);

            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnter(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnExit(go); });
            AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
            AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });
            AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });

            _inventoryObject.Slots[i]._slotUI = go;
            _slotUIs.Add(go, _inventoryObject.Slots[i]);
            go.name += ": " + i;
        }
    }

    public Vector3 CalculatePosition(int i)
    {
        float x = _start.x + ((_space.x + _size.x) * (i % _numberOfColumn));
        float y = _start.y + (-(_space.y + _size.y) * (i / _numberOfColumn));

        return new Vector3(x, y, 0f);
    }
    /*
    protected override void OnRightClick(InventorySlot slot)
    {
        _inventoryObject.UseItem(slot);
    }*/
}
