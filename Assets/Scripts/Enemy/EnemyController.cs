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

        // State들을 Dictionary 에 추가
        _states.Add(EEnemyState.Idle, new IdleState());
        _states.Add(EEnemyState.Move, new MoveState());
        _states.Add(EEnemyState.Attack, new AttackState());
        _states.Add(EEnemyState.Damage, new DamageState());
        _states.Add(EEnemyState.Die, new DeadState());

        // 기본값을 Idle로 변경
        ChangeState(EEnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        // 사망 상태로 변경
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

        // 공격 범위 안에 타겟이 들어오면 상태를 Attack 으로 전환
        if (distance < _attackRange)
        {
            ChangeState(EEnemyState.Attack);
            _curTime = _attackDelayTime;
            return;
        }

        dir.y = 0;
        dir.Normalize();

        _controller.SimpleMove(dir * _speed);

        // 이동하는 방향으로 부드럽게 회전
        //transform.forward = Vector3.Lerp(transform.forward, dir, 10 * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
    }

    // Visual Debugging을 위한 함수
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
            Debug.Log("공격!!!");
        }

        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance > _attackRange)
        {
            ChangeState(EEnemyState.Move);
            _anim.SetTrigger("Move");
        }
    }

    // 일정 시간 지나면 상태를 Idle로 전환
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

    // 피격 당했을 때 호출되는 함수
    public void OnDamageProcess() { _hp--; }

    public void Die()
    {
        // 나중에 쉐이더를 이용해서 타들어가는 것처럼 해주고 적 비활성화로 바꿈 (오브젝트 풀링 사용)
        
        gameObject.SetActive(false);
    }

    public void ChangeState(EEnemyState stateType)
    {
        // state에서 탈출
        _states[_eState].Exit(this);

        // state 변경
        _eState = stateType;

        // state 입장
        _states[_eState].Enter(this);
    }
}