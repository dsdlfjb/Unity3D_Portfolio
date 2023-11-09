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
    public int _priority;       // � �ִϸ��̼��� �켱���� ��������

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
        _calCoolTime = _coolTime;    // �ٷ� ������ �ȵǰ� ��Ÿ���� ���߿� ���� ����
    }

    // Update is called once per frame
    void Update()
    {
        if (_calCoolTime < _coolTime)
            _calCoolTime += Time.deltaTime;
    }

    public abstract void ExecuteAttack(GameObject target = null, Transform startPoint = null);
}
