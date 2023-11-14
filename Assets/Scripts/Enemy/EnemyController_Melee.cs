using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_Melee : EnemyController, IAttackable, IDamagable
{
    [SerializeField] public Transform _hitPoint;
    public Transform[] _wayPoints;

    [SerializeField] NPCBattleUI _healthBar;

    public float _maxHealth => 100f;
    float _health;

    int _hitTriggerHash = Animator.StringToHash("HitTrigger");
    int _isAliveHash = Animator.StringToHash("IsAlive");

    public override bool IsAvailableAttack
    {
        get
        {
            if (!Target) return false;

            float dist = Vector3.Distance(transform.position, Target.position);
            return (dist <= AttackRange);
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _stateMachine.AddState(new MoveState());
        _stateMachine.AddState(new AttackState());
        _stateMachine.AddState(new DeadState());

        _health = _maxHealth;

        if (_healthBar)
        {
            _healthBar.MinimumValue = 0f;
            _healthBar.MaximumValue = _maxHealth;
            _healthBar.Value = _health;
        }
    }

    private void OnAnimatorMove()
    {
        if (_anim.GetBool("IsMove") == true)
        {
            Vector3 position = transform.position;
            position.y = _savePosition.y;

            _anim.rootPosition = position;
            _agent.nextPosition = position;
        }

        else
            _agent.nextPosition = transform.position;
    }

    #region IDamageable Interfaces
    public bool IsAlive => (_health > 0);

    public void TakeDamage(int damage, GameObject hitEffectPrefab)
    {
        if (!IsAlive) return;

        _health -= damage;

        if (_healthBar)
            _healthBar.Value = _health;

        if (hitEffectPrefab)
            Instantiate(hitEffectPrefab, _hitPoint);

        if (IsAlive)
            _anim?.SetTrigger(_hitTriggerHash);

        else
        {
            if (_healthBar != null)
                _healthBar.enabled = false;

            _stateMachine.ChangeState<DeadState>();
        }
    }
    #endregion IDamageable Interfaces

    #region IAttackable Interfaces
    public GameObject _hitEffectPrefab = null;

    [SerializeField]
    List<AttackBehaviour> _attackBehaviours = new List<AttackBehaviour>();

    public AttackBehaviour CurrentAttackBehaviour { get; private set; }

    public void OnExecuteAttack(int attackIndex) { }
    #endregion IAttackable Interfaces
}
