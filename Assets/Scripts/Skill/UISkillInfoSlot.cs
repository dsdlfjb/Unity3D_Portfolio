using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UISkillInfoSlot : MonoBehaviour
{
    public List<UISkillInfoSlot> proceedSkillSlot = new List<UISkillInfoSlot>();

    [SerializeField]
    public SkillInfo info;

    [SerializeField]
    private Image skillIcon;

    [SerializeField]
    private TMP_Text skillpointText;

    [SerializeField]
    private Button button;

    [SerializeField]
    public bool isQuickSlot;


    public void Awake()
    {
        button?.onClick.AddListener(OnButton);
    }
    public bool IsEmpty()
    {

        if(info.skillName != null)
        {
            return info.skillName.Length < 1;
        }
        return true;
    }
    
    public void SetSlot()
    {

        if (info != null && !IsEmpty())
        {
            if(button)
                button.interactable = true;

            foreach (var item in proceedSkillSlot)
            {
                if (item.info.curSkillPoint == 0 && button)
                    button.interactable = false;
            }

            if(info.curSkillPoint == 0)
            {
                skillIcon.color = Color.white / 2;
            }
            else
            {
                skillIcon.color = Color.white;

                if (isQuickSlot)
                {

                    skillIcon.type = Image.Type.Filled;
                    if (info.curCooldown <= info.cooldown) info.curCooldown += Time.deltaTime;
                    if (info.cooldown != 0)
                        skillIcon.fillAmount = info.curCooldown / info.cooldown;
                }
            }

            skillIcon.sprite = info.skillIcon;
            if (skillpointText)
                skillpointText.text = $"{info.curSkillPoint}/{info.maxPoint}";
        }
        else
        {
            skillIcon.color = Color.white * 0;
        }
    }
    public void OnButton()
    {
        if (PlayerController.instance._skillPoint < 1) return;
        if (info.curSkillPoint < info.maxPoint)
        {
            PlayerController.instance._skillPoint--;
            info.curSkillPoint++;
            
        }
    }
}