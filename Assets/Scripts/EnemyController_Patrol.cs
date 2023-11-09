using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_Patrol : EnemyController, IDamageable
{
    public Collider _weaponCollider;
    public Transform _hitPoint;
    public GameObject _hitEffect = null;

    public Transform[] _waypoints;

    public NPCBattleUI _healthBar;

    protected override void Start()
    {
        base.Start();

        _stateMachine.AddState(new MoveState());
        _stateMachine.AddState(new PatrolState());

        _health = _maxHealth;

        if (_healthBar)
        {
            _healthBar = GetComponent<NPCBattleUI>();
            _healthBar.MinimumValue = 0f;
            _healthBar.MaximumValue = _maxHealth;
            _healthBar.Value = _health;
        }
    }

    public override bool IsAvailableAttack
    {
        get
        {
            if (!Target) return false;

            float distance = Vector3.Distance(transform.position, Target.position);
            return (distance <= AttackRange);
        }
    }

    public void EnableAttackCollider()
    {
        Debug.Log("Check Attack Event");

        if (_weaponCollider)
            _weaponCollider.enabled = true;

        StartCoroutine(Coroutine_DisableAttackCollider());
    }

    IEnumerator Coroutine_DisableAttackCollider()
    {
        yield return new WaitForFixedUpdate();

        if (_weaponCollider)
            _weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != _weaponCollider)
            return;

        if (((1 << other.gameObject.layer) & TargetMask) != 0)
        {
            Debug.Log("Attack Trigger : " + other.name);
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player?.TakeDamage(10, _hitEffect);
        }

        if (((1 << other.gameObject.layer) & TargetMask) == 0)
        {
            //It wasn't in an ignore layer
        }
    }

    #region IDamagable Interfaces
    public float _maxHealth = 100f;
    float _health;
    public bool IsAlive => (_health > 0);

    int _hitTriggerHash = Animator.StringToHash("HitTrigger");
    int _isAliveHash = Animator.StringToHash("IsAlive");

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
            _healthBar.enabled = false;
            _anim?.SetBool(_isAliveHash, false);

            Destroy(gameObject, 3.0f);
        }
    }
    #endregion IDamagable Interfaces
}
