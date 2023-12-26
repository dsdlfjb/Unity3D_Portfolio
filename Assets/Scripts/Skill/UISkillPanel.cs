using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// [KMONG] SkillPanel UI입니다
public class UISkillPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    KeyCode openKeyCode;
    [SerializeField]
    TMP_Text skillpointText;

    [SerializeField]
    List<UISkillInfoSlot> slots = new List<UISkillInfoSlot>();

    [SerializeField]
    UISkillInfoSlot dragSlot;

    [SerializeField]
    GraphicRaycaster gr;

    [SerializeField]
    CanvasGroup canvasGroup;

    public UISkillInfoSlot GetSkillSlot(string name)
    {
        return slots.Find(_ => _.info.skillName == name);
    }
    void Awake()
    {
        slots.AddRange(GetComponentsInChildren<UISkillInfoSlot>());
    }

    bool isShow = false;
    void Show()
    {
        isShow = true;

        canvasGroup.alpha = 1;
        canvasGroup.interactable =true;
        canvasGroup.blocksRaycasts = true;



    }
    void Hide()
    {
        isShow = false;


        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    void Update()
    {
        skillpointText.text = $"Skill Point : {PlayerController.instance._skillPoint}";
        foreach (var item in slots)
        {
            item.SetSlot();
        }

        if (!dragSlot.IsEmpty())
            dragSlot.transform.position = Input.mousePosition;

        if(Input.GetKeyDown(openKeyCode))
        {
            if (!isShow) Show();
            else Hide();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var slot = ReturnSlot(eventData);

        // 드래그를 시작할 데이터가 없다면
        if (slot == null)
        {
            if(!dragSlot.IsEmpty())
            {
                dragSlot.gameObject.SetActive(false);
                dragSlot.info = new SkillInfo();
            }
            return;
        }
        else
        {
            if (dragSlot.IsEmpty() && slot.info.curSkillPoint > 0)
            {
                dragSlot.info = slot.info;
                dragSlot.SetSlot();
                dragSlot.gameObject.SetActive(true);
            }
            else
            {
                if (slot.isQuickSlot)
                {
                    slot.info = dragSlot.info;
                }
                dragSlot.info = new SkillInfo();
                dragSlot.gameObject.SetActive(false);
            }
        }
    }
    private UISkillInfoSlot ReturnSlot(PointerEventData eventData)
    {
        var result = new List<RaycastResult>();
        gr.Raycast(eventData, result);

        foreach (var item in result)
        {
            if (item.gameObject.TryGetComponent(out UISkillInfoSlot slot))
            {
                return slot;
            }

        }
        return null;
    }
}
