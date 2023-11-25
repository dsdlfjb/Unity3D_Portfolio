using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EEEnemyState
{
    Idle,
    Move,
    Attack,
    Damage,
    Die
}

public class Enemy : MonoBehaviour
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
    EEEnemyState _eState;


    void Awake()
    {
        _eState = EEEnemyState.Idle;
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("현재 상태 : " + _eState);

        switch(_eState)
        {
            case EEEnemyState.Idle:
                Idle();
                break;

            case EEEnemyState.Move:
                Move();
                break;

            case EEEnemyState.Attack:
                Attack();
                break;

            case EEEnemyState.Damage:
                //Damage();
                break;

            case EEEnemyState.Die:
                //Die();
                break;
        }
    }

    public void Idle()
    {
        _curTime += Time.deltaTime;

        if (_curTime > _idleDelayTime)
        {
            _eState = EEEnemyState.Move;
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
            _eState = EEEnemyState.Attack;
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
        Vector3 dir = _target.position - transform.position;
        float distance = dir.magnitude;

        if (_curTime > _attackDelayTime)
        {
            _curTime = 0;
            _anim.SetTrigger("AttackTrigger");
        }

        if (distance > _attackRange)
        {
            _eState = EEEnemyState.Move;
            _anim.SetTrigger("Move");
            return;
        }
    }

    // 일정 시간 지나면 상태를 Idle로 전환
    private IEnumerator Coroutine_Damage()
    {
        // 1. 상태를 Damage로 전환
        _eState = EEEnemyState.Damage;
        // 2. 애니메이션 Damage 상태로 전환
        _anim.SetTrigger("HitTrigger");
        // 3. 잠시 기다리기
        yield return new WaitForSeconds(_damageDelayTime);
        // 4. 기다린 다음 상태를 Idle로 전환
        _eState = EEEnemyState.Idle;
    }

    // 피격 당했을 때 호출되는 함수
    public void OnDamageProcess()
    {
        if (_eState == EEEnemyState.Die) return;

        _hp--;
        _curTime = 0;

        StopAllCoroutines();

        if (_hp <= 0)
            StartCoroutine(Coroutine_Die());

        else
            StartCoroutine(Coroutine_Damage());
    }

    private IEnumerator Coroutine_Die()
    {
        // 나중에 쉐이더를 이용해서 타들어가는 것처럼 해주고 적 비활성화로 바꿈 (오브젝트 풀링 사용)
        _eState = EEEnemyState.Die;
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Player")
        {
            Debug.Log("Hit!!!");
        }
    }
}