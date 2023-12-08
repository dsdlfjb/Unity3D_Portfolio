using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EEnemyState
{
    Idle,
    Move,
    Attack,
    Damage,
    Die
}

public class EnemyController : MonoBehaviour
{
    [Header("Idle State")]
    public float _idleDelayTime = 2f;
    float _curTime = 0;

    [Header("Move State")]
    public float _speed = 5f;
    public Transform _target;

    [Header("Attack State")]
    public float _attackRange = 2f;
    public float _attackDelayTime = 2f;

    [Header("Damage State")]
    int _hp = 100;
    public float _damageDelayTime = 1f;

    [Header("Damage State")]
    public float _dieSpeed = 2f;
    public float _dieYPosition = -2f;

    public GameObject _hitVFX;

    CharacterController _controller;
    NPCBattleUI _hpBar;
    Animator _anim;
    NavMeshAgent _agent;

    EEnemyState _eState;


    void Awake()
    {
        _eState = EEnemyState.Idle;
        _controller = GetComponent<CharacterController>();
        _hpBar = GetComponent<NPCBattleUI>();
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _agent.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("���� ���� : " + _eState);

        switch (_eState)
        {
            case EEnemyState.Idle:
                Idle();
                break;

            case EEnemyState.Move:
                Move();
                break;

            case EEnemyState.Attack:
                Attack();
                break;

            case EEnemyState.Damage:
                //Damage();
                break;

            case EEnemyState.Die:
                //Die();
                break;
        }
    }

    public void Idle()
    {
        _curTime += Time.deltaTime;

        if (_curTime > _idleDelayTime)
        {
            _eState = EEnemyState.Move;
            _anim.SetTrigger("Move");
            _curTime = 0;
        }
    }

    public void Move()
    {
        if (_agent.enabled == false)
            _agent.enabled = true;

        Vector3 dir = _target.position - transform.position;
        float distance = dir.magnitude;

        // ���� ���� �ȿ� Ÿ���� ������ ���¸� Attack ���� ��ȯ
        if (distance < _attackRange)
        {
            _eState = EEnemyState.Attack;
            _curTime = _attackDelayTime;

            // ��ã�� ����
            _agent.enabled = false;
            return;
        }

        // Agent�� �̿��� ��ã��
        _agent.destination = _target.position;

    }

    // Visual Debugging�� ���� �Լ�
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        /*
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _aggroRange);
        */
    }

    public void Attack()
    {
        _curTime += Time.deltaTime;
        Vector3 dir = _target.position - transform.position;
        float distance = dir.magnitude;

        if (_curTime > _attackDelayTime)
        {
            _curTime = 0;
            _anim.SetTrigger("AttackTrigger");
        }

        if (distance > _attackRange)
        {
            _eState = EEnemyState.Move;
            _anim.SetTrigger("Move");
            return;
        }
    }

    // ���� �ð� ������ ���¸� Idle�� ��ȯ
    private IEnumerator Coroutine_Damage()
    {
        // 1. ���¸� Damage�� ��ȯ
        _eState = EEnemyState.Damage;
        // 2. �ִϸ��̼� Damage ���·� ��ȯ
        _anim.SetTrigger("HitTrigger");
        // 3. ��� ��ٸ���
        yield return new WaitForSeconds(_damageDelayTime);
        // 4. ��ٸ� ���� ���¸� Idle�� ��ȯ
        _eState = EEnemyState.Idle;
    }

    // �ǰ� ������ �� ȣ��Ǵ� �Լ�
    public void TakeDamage(int damageAmount)
    {
        if (_eState == EEnemyState.Die) return;

        _agent.enabled = false;
        _hp -= damageAmount;
        _hpBar.Value -= damageAmount;
        _hpBar.TakeDamage(damageAmount);
        _anim.SetTrigger("HitTrigger");
        _curTime = 0;

        StopAllCoroutines();

        if (_hp <= 0)
            StartCoroutine(Coroutine_Die());

        else
            StartCoroutine(Coroutine_Damage());
    }

    private IEnumerator Coroutine_Die()
    {
        // ���߿� ���̴��� �̿��ؼ� Ÿ���� ��ó�� ���ְ� �� ��Ȱ��ȭ�� �ٲ� (������Ʈ Ǯ�� ���)
        _eState = EEnemyState.Die;
        _anim.SetTrigger("Die");
        _controller.enabled = false;

        yield return new WaitForSeconds(2);

        while (transform.position.y < _dieYPosition)
        {
            transform.position += Vector3.down * _dieSpeed * Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(_hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);
    }
}