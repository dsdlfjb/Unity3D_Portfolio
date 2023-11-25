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
        Debug.Log("���� ���� : " + _eState);

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

        // ���� ���� �ȿ� Ÿ���� ������ ���¸� Attack ���� ��ȯ
        if (distance < _attackRange)
        {
            _eState = EEEnemyState.Attack;
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

    // ���� �ð� ������ ���¸� Idle�� ��ȯ
    private IEnumerator Coroutine_Damage()
    {
        // 1. ���¸� Damage�� ��ȯ
        _eState = EEEnemyState.Damage;
        // 2. �ִϸ��̼� Damage ���·� ��ȯ
        _anim.SetTrigger("HitTrigger");
        // 3. ��� ��ٸ���
        yield return new WaitForSeconds(_damageDelayTime);
        // 4. ��ٸ� ���� ���¸� Idle�� ��ȯ
        _eState = EEEnemyState.Idle;
    }

    // �ǰ� ������ �� ȣ��Ǵ� �Լ�
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
        // ���߿� ���̴��� �̿��ؼ� Ÿ���� ��ó�� ���ְ� �� ��Ȱ��ȭ�� �ٲ� (������Ʈ Ǯ�� ���)
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