using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollision : MonoBehaviour
{
    [SerializeField] GameObject _attackCollision;
 
    private void OnEnable()
    {
        StartCoroutine(Coroutine_AutoDisable());
    }

    public void OnAttackCollision()
    {
        _attackCollision.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ Ÿ���ϴ� ����� �±�, ������Ʈ, �Լ��� �ٲ� �� �ִ�
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(10);
        }
    }

    private IEnumerator Coroutine_AutoDisable()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }
}
