using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehaviour : MonoBehaviour
{
#if UNITY_EDITOR
    [Multiline]
    public string _developmentDescription = "";
#endif

    public int _animationIndex;
    public int _priority;       // 어떤 애니메이션을 우선으로 실행할지

    public int _damage = 10;
    public float _range = 3f;

    [SerializeField] float _coolTime;
    protected float _calCoolTime = 0f;

    public GameObject _effectPrefab;

    [HideInInspector]
    public LayerMask _targetMask;

    [SerializeField]
    public bool IsAvailable => _calCoolTime >= _coolTime;

    // Start is called before the first frame update
    void Start()
    {
        _calCoolTime = _coolTime;    // 바로 공격이 안되게 쿨타임을 나중에 수정 가능
    }

    // Update is called once per frame
    void Update()
    {
        if (_calCoolTime < _coolTime)
            _calCoolTime += Time.deltaTime;
    }

    public abstract void ExecuteAttack(GameObject target = null, Transform startPoint = null);
}
