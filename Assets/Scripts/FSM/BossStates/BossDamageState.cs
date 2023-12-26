using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageState : BossBaseState
{
    public override void Enter(DragonController target)
    {
        target.StartCoroutine(nameof(Coroutine_Damage), target);
    }

    public override void Execute(DragonController target)
    {
        
    }

    public override void Exit(DragonController target)
    {
        
    }

    private IEnumerator Coroutine_Damage(DragonController target)
    {
        target.Anim.SetTrigger("Hit");
    
        target.StartCoroutine(Coroutine_OnHitColor(target));
    
        // 3. ��� ��ٸ���
        yield return new WaitForSeconds(target._damageDelayTime);
        // 4. ��ٸ� ���� ���¸� Idle�� ��ȯ
        target.ChangeState(BossStateEnum.Chase);
    }
    private IEnumerator Coroutine_OnHitColor(DragonController target)
    {
        // ���� ���������� ������ �� 0.1�� �Ŀ� ���� �������� ����
        target.MeshRenderer.material.color = Color.red;
    
        yield return new WaitForSeconds(0.1f);
    
        target.MeshRenderer.material.color = target.OriginColor;
    }
}
