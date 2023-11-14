using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_Range : EnemyController, IAttackable, IDamagable
{
    [SerializeField] 
    public Transform _hitPoint;
    public Transform[] _wayPoints;

    public override float AttackRange => CurrentAttackBehaviour?._range ?? 6f;

    [SerializeField]
    NPCBattleUI _battleUI;

    public float _maxHealth => 100f;
    float _health;

    int _hitTriggerHash = Animator.StringToHash("HitTrigger");

    [SerializeField] Transform _projectilePoint;
    [SerializeField] GameObject _hitEffectPrefab;

    AttackBehaviour _attackBehaviour;
    PlayerController _player;

    public override bool IsAvailableAttack
    {
        get
        {
            if (!Target)
                return false;

            float distance = Vector3.Distance(transform.position, Target.position);
            return (distance <= AttackRange);
        }
    }

    protected override void Start()
    {
        base.Start();

        _stateMachine.AddState(new MoveState());
        _stateMachine.AddState(new AttackState());
        _stateMachine.AddState(new DeadState());

        _health = _maxHealth;

        if (_battleUI)
        {
            _battleUI.MinimumValue = 0f;
            _battleUI.MaximumValue = _maxHealth;
            _battleUI.Value = _health;
        }

        InitAttackBehaviour();
    }

    protected override void Update()
    {
        CheckAttackBehaviour();

        base.Update();
    }
    
    private void OnAnimatorMove()
    {
        Vector3 position = transform.position;
        position.y = _savePosition.y;

        _anim.rootPosition = position;
        _agent.nextPosition = position;
    }

    void InitAttackBehaviour()
    {
        foreach (AttackBehaviour behaviour in _attackBehaviours)
        {
            if (CurrentAttackBehaviour == null)
            {
                CurrentAttackBehaviour = behaviour;
            }

            behaviour._targetMask = TargetMask;
        }
    }

    void CheckAttackBehaviour()
    {
        if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable)
        {
            CurrentAttackBehaviour = null;

            foreach (AttackBehaviour behaviour in _attackBehaviours)
            {
                if (behaviour.IsAvailable)
                {
                    if ((CurrentAttackBehaviour == null) || (CurrentAttackBehaviour._priority < behaviour._priority))
                        CurrentAttackBehaviour = behaviour;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            if(_player._isAttack)
            {
                TakeDamage(_attackBehaviour._damage, _hitEffectPrefab);
            }
        }
    }

    #region IDamageable Interfaces
    public bool IsAlive => (_health > 0);

    public void TakeDamage(int damage, GameObject hitEffectPrefab)
    {
        if (!IsAlive) return;

        _health -= damage;

        if (_battleUI)
        {
            _battleUI.Value = _health;
            _battleUI.TakeDamage(damage);
        }

        if (hitEffectPrefab)
            Instantiate(hitEffectPrefab, _hitPoint);

        if (IsAlive)
            _anim?.SetTrigger(_hitTriggerHash);

        else
        {
            if (_battleUI != null)
                _battleUI.enabled = false;

            _stateMachine.ChangeState<DeadState>();
        }
    }
    #endregion IDamageable Interfaces

    #region IAttackable Interfaces
    [SerializeField]
    private List<AttackBehaviour> _attackBehaviours = new List<AttackBehaviour>();

    public AttackBehaviour CurrentAttackBehaviour { get; private set; }

    public void OnExecuteAttack(int attackIndex)
    {
        if (CurrentAttackBehaviour != null && Target != null)
        {
            CurrentAttackBehaviour.ExecuteAttack(Target.gameObject, _projectilePoint);
        }
    }
    #endregion IAttackable Interfaces
}
