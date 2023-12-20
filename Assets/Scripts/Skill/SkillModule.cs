using System.Collections.Generic;
using UnityEngine;

// [KMONG] 스킬모듈 , 플레이어 한테 장착
class SkillModule : MonoBehaviour
{
    [SerializeField]
    List<UISkillInfoSlot> skillPanel = new List<UISkillInfoSlot>();
    
    private void UseSkill(int i)
    {
        if (!skillPanel[i].IsEmpty())
        {
            if (skillPanel[i].info.curCooldown > skillPanel[i].info.cooldown)
            {
                skillPanel[i].info.curCooldown = 0;


                if (PlayerController.instance._mana < skillPanel[i].info.mana) return;

                PlayerController.instance._mana -= (int)skillPanel[i].info.mana;
                var obj = Instantiate(skillPanel[i].info.skill, transform) as Skill;
                obj.CastSkill(skillPanel[i].info);
            }
        }
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseSkill(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseSkill(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseSkill(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UseSkill(3);
        }
    }



}