using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyState
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

    CharacterController _controller;
    Animator _anim;
    EEnemyState _eState;

    Dictionary<EEnemyState, EnemyState> _states = new Dictionary<EEnemyState, EnemyState>();

    void Awake()
    {
        _eState = EEnemyState.Idle;
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();

        // State���� Dictionary �� �߰�
        _states.Add(EEnemyState.Idle, new IdleState());
        _states.Add(EEnemyState.Move, new MoveState());
        _states.Add(EEnemyState.Attack, new AttackState());
        _states.Add(EEnemyState.Damage, new DamageState());
        _states.Add(EEnemyState.Die, new DeadState());

        // �⺻���� Idle�� ����
        ChangeState(EEnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        // ��� ���·� ����
        if (_hp <= 0) ChangeState(EEnemyState.Die);

        _states[_eState].Execute(this);
    }

    public void Idle()
    {
        _curTime += Time.deltaTime;

        if (_curTime > _idleDelayTime)
        {
            ChangeState(EEnemyState.Move);
            _anim.SetTrigger("Move");
            _curTime = 0;
        }
    }

    public void Move()
    {
        Vector3 dir = _target.position - transform.position;
        float distance = dir.magnitude;

        // ���� ���� �ȿ� Ÿ���� ������ ���¸� Attack ���� ��ȯ
        if (distance < _attackRange)
        {
            ChangeState(EEnemyState.Attack);
            _curTime = _attackDelayTime;
            return;
        }

        dir.y = 0;
        dir.Normalize();

        _controller.SimpleMove(dir * _speed);

        // �̵��ϴ� �������� �ε巴�� ȸ��
        //transform.forward = Vector3.Lerp(transform.forward, dir, 10 * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
    }

    // Visual Debugging�� ���� �Լ�
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

    public void Attack()
    {
        _curTime += Time.deltaTime;

        if (_curTime > _attackDelayTime)
        {
            _curTime = 0;
            _anim.SetTrigger("AttackTrigger");
            Debug.Log("����!!!");
        }

        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance > _attackRange)
        {
            ChangeState(EEnemyState.Move);
            _anim.SetTrigger("Move");
        }
    }

    // ���� �ð� ������ ���¸� Idle�� ��ȯ
    public void Damage()
    {
        _curTime += Time.deltaTime;

        if (_curTime > _damageDelayTime)
        {
            _curTime = 0;
            ChangeState(EEnemyState.Idle);
        }

        OnDamageProcess();
    }

    // �ǰ� ������ �� ȣ��Ǵ� �Լ�
    public void OnDamageProcess() { _hp--; }

    public void Die()
    {
        // ���߿� ���̴��� �̿��ؼ� Ÿ���� ��ó�� ���ְ� �� ��Ȱ��ȭ�� �ٲ� (������Ʈ Ǯ�� ���)
        
        gameObject.SetActive(false);
    }

    public void ChangeState(EEnemyState stateType)
    {
        // state���� Ż��
        _states[_eState].Exit(this);

        // state ����
        _eState = stateType;

        // state ����
        _states[_eState].Enter(this);
    }
}