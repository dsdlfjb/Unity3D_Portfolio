using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Idle State")] public float _idleDelayTime = 2f;
    public float _curTime = 0;

    [Header("Move State")] public float _speed = 5f;
    public Transform _target;

    [Header("Attack State")] 
    public GameObject _attackDealObject;
    public float _attackRange = 2f;
    public float _attackDelayTime = 2f;

    [Header("Damage State")] int _hp = 100;
    private int _maxHp;
    public float _damageDelayTime = 1f;

    [Header("Damage State")] public float _dieSpeed = 2f;
    public float _dieYPosition = -2f;

    [Header("Respawn")]
    public float _respawnTime = 3;
    public Vector3 _respawnVec;

    CharacterController _controller;
    NPCBattleUI _hpBar;
    Animator _anim;
    public Animator Anim
    {
        get
        {
            return _anim;
        }
    }
    NavMeshAgent _agent;
    public NavMeshAgent Agent
    {
        get { return _agent; }
    }
    SkinnedMeshRenderer _meshRenderer;
    public SkinnedMeshRenderer MeshRenderer
    {
        get { return _meshRenderer; }
    }
    Color _originColor;
    public Color OriginColor
    {
        get
        {
            return _originColor;
        }
    }

    EEnemyState _eState;
    EnemyState enemyState;
    Dictionary<EEnemyState,EnemyState> enemyStates = new Dictionary<EEnemyState, EnemyState>();
    
    void Awake()
    {
        _respawnVec = transform.position;
        _controller = GetComponent<CharacterController>();
        _hpBar = GetComponent<NPCBattleUI>();
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _target = FindObjectOfType<PlayerController>().transform;
        _originColor = _meshRenderer.material.color;
        _agent.enabled = false;
        _maxHp = _hp;

        enemyStates = new Dictionary<EEnemyState, EnemyState>();
        enemyStates.Add(EEnemyState.Idle,new IdleState());
        enemyStates.Add(EEnemyState.Move,new MoveState());
        enemyStates.Add(EEnemyState.Attack,new AttackState());
        enemyStates.Add(EEnemyState.Damage,new DamageState());
        enemyStates.Add(EEnemyState.Die,new DeadState());
    }

    private void OnEnable()
    {
        _hp = _maxHp;
        
        _eState = EEnemyState.Idle;
        enemyState = enemyStates[_eState];
    }

    // 상태 변경 메소드
    public void ChangeState(EEnemyState newState)
    {
        enemyState.Exit(this);
        _eState = newState;
        enemyState = enemyStates[newState];
        enemyState.Enter(this);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Enemy State : " + _eState);

        if (_eState != EEnemyState.Die && _eState != EEnemyState.Damage )
        {
            enemyState.Execute(this);
        }
    }


    public void Attack()
    {
        _curTime = 0;
        _anim.SetTrigger("AttackTrigger");
    }

    // 몬스터가 데미지를 받는 함수
    public void TakeDamage(int damageAmount)
    {
        if (_eState == EEnemyState.Die) return;
    
        _agent.enabled = false;

        _hp -= damageAmount;
        _curTime = 0;
    
        StopAllCoroutines();

        if (_hp <= 0)
        {
            ChangeState(EEnemyState.Die);
        }

        else
        {
            ChangeState(EEnemyState.Damage);
        }

        Debug.Log($"cur Enemy HP {_hp}");
    }

    public void RespawnObj()
    {
        transform.position = _respawnVec;
        gameObject.SetActive(true);
    }

    public void StartDealDamage()
    {
        _attackDealObject.GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        _attackDealObject.GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
    }
}