using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public abstract class InventoryUI : MonoBehaviour
{
    public InventoryObject _inventoryObject;        // ���� �κ��丮 ������Ʈ
    InventoryObject _previousInventory;     // ���� Ȱ���� ������ �κ��丮 ������Ʈ

    public Dictionary<GameObject, InventorySlot> _slotUIs = new Dictionary<GameObject, InventorySlot>();

    private void Awake()
    {
        CreateSlots();

        for (int i = 0; i < _inventoryObject.Slots.Length; i++)
        {
            _inventoryObject.Slots[i]._parent = _inventoryObject;
            _inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;
        }

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        for (int i = 0; i < _inventoryObject.Slots.Length; i++)
        {
            _inventoryObject.Slots[i].UpdateSlot(_inventoryObject.Slots[i]._item, _inventoryObject.Slots[i]._amount);
        }
    }

    public abstract void CreateSlots();

    public void OnPostUpdate(InventorySlot slot)
    {
        slot._slotUI.transform.GetChild(0).GetComponent<Image>().sprite = slot._item._id < 0 ? null : slot.ItemObject._icon;
        slot._slotUI.transform.GetChild(0).GetComponent<Image>().color = slot._item._id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        slot._slotUI.GetComponentInChildren<TextMeshProUGUI>().text = slot._item._id < 0 ? string.Empty : (slot._amount == 1 ? string.Empty : slot._amount.ToString("n0"));
    }

    protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetComponent<EventTrigger>();

        if (!trigger)
        {
            Debug.LogWarning("No EventTrigger component found!");
            return;
        }

        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnterInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = go.GetComponent<InventoryUI>();
    }
    public void OnExitInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = null;
    }


    public void OnEnter(GameObject go)
    {
        MouseData.slotHoveredOver = go;
        MouseData.interfaceMouseIsOver = go.GetComponentInParent<InventoryUI>();
    }

    public void OnExit(GameObject go)
    {
        MouseData.slotHoveredOver = null;
    }


    public void OnStartDrag(GameObject go)
    {
        MouseData.tempItemBeingDragged = CreateDragImage(go);
    }

    GameObject CreateDragImage(GameObject go)
    {
        // �κ��丮 ���Կ� ���� Ű���� ã�� ���ϸ� null�� ��ȯ
        if (_slotUIs[go]._item._id < 0)
            return null;

        // �׷��� �ʴٸ� �� ���ӿ�����Ʈ�� ����
        GameObject dragImage = new GameObject();

        RectTransform rectTransform = dragImage.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(50, 50);
        dragImage.transform.SetParent(transform.parent);        // canvas�� drag�̹����� parent�� ����
        Image image = dragImage.AddComponent<Image>();
        image.sprite = _slotUIs[go].ItemObject._icon;
        image.raycastTarget = false;

        dragImage.name = "Drag Image";

        return dragImage;
    }

    // ����� �̹��� �������� ���콺 Ŀ�� ��ġ�� ����ٴϰ� �����ϴ� �Լ�
    public void OnDrag(GameObject go)
    {
        if (MouseData.tempItemBeingDragged == null)
            return;

        MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    // �ӽ� �̹��� �������� �����ϱ� ���� �Լ�
    public void OnEndDrag(GameObject go)
    {
        Destroy(MouseData.tempItemBeingDragged);

        // �κ��丮UI ���� ���� ������ �������� ����
        if (MouseData.interfaceMouseIsOver == null)
            _slotUIs[go].RemoveItem();

        // �������̽� ���ο��� ���� ���� ������ �������� ��ü
        else if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver._slotUIs[MouseData.slotHoveredOver];
            _inventoryObject.SwapItems(_slotUIs[go], mouseHoverSlotData);
        }
    }

    public void OnClick(GameObject go, PointerEventData data)
    {
        InventorySlot slot = _slotUIs[go];

        if (slot == null) return;

        if (data.button == PointerEventData.InputButton.Left)
            OnLeftClick(slot);

        else if (data.button == PointerEventData.InputButton.Right)
            OnRightClick(slot);
    }

    protected virtual void OnRightClick(InventorySlot slot) { }

    protected virtual void OnLeftClick(InventorySlot slot) { }
}
