using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [KMONG] SkillQucikSlot UIÀÔ´Ï´Ù
public class UISkillQuickSlot : MonoBehaviour
{
    SkillInfo skill;
    Image skillIcon;
    float curCooldown;

    public void Update()
    {
        if (skill == null) return;


        curCooldown += Time.deltaTime;
    }

    public bool IsUsable() => (skill == null);
}
