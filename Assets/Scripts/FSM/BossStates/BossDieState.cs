using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDieState : BossBaseState
{
    public override void Enter(DragonController target)
    {
        // 죽었으니 못움직이게
        target.Agent.enabled = false;
        // 코루틴 시작
        target.StartCoroutine(Coroutine_Die(target));
    }

    public override void Execute(DragonController target) { }

    IEnumerator Coroutine_Die(DragonController target)
    {
        // 죽는 애니메이션 실행
        target.Anim.SetTrigger("Die");
        // 몇초 뒤 오브젝트 비활성화
        yield return new WaitForSeconds(3);
        target.gameObject.SetActive(false);
    }

    public override void Exit(DragonController target)
    {
        // 죽음 처리
    }
}
