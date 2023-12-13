using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : EnemyState
{
    public override void Enter(EnemyController target)
    {
        target.StartCoroutine(Coroutine_Die(target));
    }

    IEnumerator Coroutine_Die(EnemyController target)
    {
        // 데미지를 받는 상태인지 아닌지 확인
        target.Anim.SetTrigger("Die");
        target.Agent.enabled = false;
        GameManager.Instance.GetExp();
        UIManager.Instance.EXP_UP();

        yield return new WaitForSeconds(3);
    
        while (target.transform.position.y < target._dieYPosition)
        {
            target.transform.position += Vector3.down * (target._dieSpeed * Time.deltaTime);
            yield return null;
        }
    
        target.gameObject.SetActive(false);
        target.Invoke("RespawnObj", target._respawnTime);
    }

    public override void Execute(EnemyController target) { }

    public override void Exit(EnemyController target) { }
}