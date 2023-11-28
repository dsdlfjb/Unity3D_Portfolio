using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInventoryUI : InventoryUI
{
    public GameObject[] _staticSlots = null;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public override void CreateSlots()
    {
        _slotUIs = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < _inventoryObject.Slots.Length; i++)
        {
            GameObject slotGO = _staticSlots[i];

            AddEvent(slotGO, EventTriggerType.PointerEnter, delegate { OnEnter(slotGO); });
            AddEvent(slotGO, EventTriggerType.PointerExit, delegate { OnExit(slotGO); });
            AddEvent(slotGO, EventTriggerType.BeginDrag, delegate { OnStartDrag(slotGO); });
            AddEvent(slotGO, EventTriggerType.EndDrag, delegate { OnEndDrag(slotGO); });
            AddEvent(slotGO, EventTriggerType.Drag, delegate { OnDrag(slotGO); });

            _inventoryObject.Slots[i]._slotUI = slotGO;
            _slotUIs.Add(slotGO, _inventoryObject.Slots[i]);
        }
    }
}
