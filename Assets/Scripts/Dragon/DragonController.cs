using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DragonController : MonoBehaviour
{
    public int _hp = 1000;
    public int _maxHp = 1000;

    // Wakeup Distance
    public float wakeupDistance;
    [Header("AttackState")]
    // All Attack Distance
    public float allAttackDistance;
    // MouthAttack Distance;
    public float mouthAttackDistance;
    //Breathe Distance
    public float breatheAttackDistance;
    //Attack Time
    public float _attackTimer;
    // maxAttackTime
    public float _maxAttackTime;

    [Header("Boss DamageState")]
    // Boss damage 
    public float _damageDelayTime;
    // target
    public Transform _target;

    public Color OriginColor;
    
    // player distance
    private Vector3 distanceDir;
    public float distance;
    
    //UI
    public Slider bossHpBar;

    [Header("Attacks")] public GameObject MouthAttacks;
    public GameObject BreatheObject;
    public ParticleSystem _particlce;

    // componenets
    Animator _anim;
    public Animator Anim
    {
        get
        {
            return _anim;
        }
    }
    private NavMeshAgent _agent;
    public NavMeshAgent Agent
    {
        get
        {
            return _agent;
        }
    }
    private SkinnedMeshRenderer _meshRenderer;
    public SkinnedMeshRenderer MeshRenderer
    {
        get
        {
            return _meshRenderer;
        }
    }

    private Dictionary<BossStateEnum, BossBaseState> bossState;
    // 보스의 현재 상태
    private BossStateEnum _bossStateEnum;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _target = FindObjectOfType<PlayerController>().transform;
        _agent = GetComponent<NavMeshAgent>();
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        OriginColor = _meshRenderer.material.color;

        // 이 이후 상태들 추가
        bossState = new Dictionary<BossStateEnum, BossBaseState>();
        bossState.Add(BossStateEnum.Sleep, new BossSleepState()); 
        bossState.Add(BossStateEnum.Idle, new BossIdleState());
        bossState.Add(BossStateEnum.Chase, new BossChaseState());
        bossState.Add(BossStateEnum.Attack, new BossAttackState());
        bossState.Add(BossStateEnum.Die, new BossDieState());
    }

    private void OnEnable()
    {
        _bossStateEnum = BossStateEnum.Sleep;
    }

    public void ChangeState(BossStateEnum newState)
    {
        bossState[_bossStateEnum].Exit(this);
        _bossStateEnum = newState;
        bossState[_bossStateEnum].Enter(this);
    }

    void Update()
    {
        Debug.Log(_bossStateEnum);
        if (_bossStateEnum != BossStateEnum.Die && _bossStateEnum != BossStateEnum.Damage)
        {
            // 거리는 계속 체크
            distanceDir = _target.position - transform.position;
            distance = distanceDir.magnitude;
            bossState[_bossStateEnum].Execute(this);
        }
    }

    #region MouthAttack

    // 물기 시작
    public void MouthAttackStart()
    {
        MouthAttacks.SetActive(true);
    }
    
    // 물기끝
    public void MouthAttackEnd()
    {
        MouthAttacks.SetActive(false);
    }

    #endregion

    #region BreathAttack

         // 물기 시작
        public void BreatheAttackStart()
        {
            BreatheObject.SetActive(true);
        }
        
        // 물기끝
        public void BreatheAttackEnd()
        {
            BreatheObject.SetActive(false);
        }

    #endregion
    
    
    

    public void TakeDamage(int damageAmount)
    {
        _hp -= damageAmount;

        if (bossHpBar != null)
        {
            bossHpBar.value = _hp / _maxHp;
        }

        Debug.Log($"cur Dragon HP {_hp}");
        if (_hp <= 0)
        {
            ChangeState(BossStateEnum.Die);
        }

        else
        {
            ChangeState(BossStateEnum.Damage);
        }
        
    }
}
