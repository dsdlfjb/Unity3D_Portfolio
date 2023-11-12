// 플레이어 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IAttackable, IDamagable
{
    public static PlayerController instance;

    public string _currentMapName;      // Portal 스크립트에 있는 transferMapName 변수의 값을 저장

    Animator _anim;
    Camera _camera;
    CharacterController _controller;

    [SerializeField] float _speed = 5f;
    [SerializeField] float _runSpeed = 8f;
    [SerializeField] float _finalSpeed;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _jumpForce = 3f;
    [SerializeField] float _smoothness = 10f;

    [SerializeField] bool _isRun;
    [SerializeField] bool _toggleCameraRotation;

    Vector3 _moveDirection;

    public float _maxHealth = 100f;
    protected float _health;

    public Transform _target;
    [SerializeField] Transform _hitPoint;

    readonly int _moveHash = Animator.StringToHash("Move");
    readonly int _moveSpeedHash = Animator.StringToHash("MoveSpeed");
    //readonly int fallingHash = Animator.StringToHash("Falling");
    readonly int _attackTriggerHash = Animator.StringToHash("AttackTrigger");
    readonly int _attackIndexHash = Animator.StringToHash("AttackIndex");
    readonly int _hitTriggerHash = Animator.StringToHash("HitTrigger");
    readonly int _isAliveHash = Animator.StringToHash("IsAlive");

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            _anim = this.GetComponent<Animator>();
            _camera = Camera.main;
            _controller = this.GetComponent<CharacterController>();
            instance = this;
        }

        else
            Destroy(this.gameObject);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
            _toggleCameraRotation = true;       // 둘러보기 활성화

        else
            _toggleCameraRotation = false;      // 둘러보기 비활성화

        if (Input.GetKey(KeyCode.LeftShift))
            _isRun = true;

        else
            _isRun = false;

        InputMovement();

        if (_controller.isGrounded == false)
            _moveDirection.y += _gravity * Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (_toggleCameraRotation != true)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * _smoothness);
        }
    }

    void InputMovement()
    {
        _finalSpeed = (_isRun) ? _runSpeed : _speed;

        Vector3 forword = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDir = right * Input.GetAxisRaw("Horizontal") + forword * Input.GetAxisRaw("Vertical");

        _controller.Move(moveDir.normalized * _finalSpeed * Time.deltaTime);

        float percent = ((_isRun) ? 1 : 0.5f) * moveDir.magnitude;

        _anim.SetFloat("Speed", percent, 0.1f, Time.deltaTime) ;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpTo();
            _anim.SetTrigger("DoJump");
        }
    }

    void JumpTo()
    {
        if (_controller.isGrounded == true)
            _moveDirection.y = _jumpForce;
    }

    #region IAttackable Interfaces
    [SerializeField] List<AttackBehaviour> _attackBehaviours = new List<AttackBehaviour>();

    public AttackBehaviour CurrentAttackBehaviour { get; private set; }

    public void OnExecuteAttack(int attackIndex)
    {
        if (CurrentAttackBehaviour != null)
            CurrentAttackBehaviour.ExecuteAttack(_target.gameObject);
    }
    #endregion IAttackable Interfaces

    #region IDamagable Interfaces
    public bool IsAlive => _health > 0;

    public void TakeDamage(int damage, GameObject damageEffectPrefab)
    {
        if (!IsAlive)
        {
            return;
        }

        _health -= damage;

        if (damageEffectPrefab)
        {
            Instantiate<GameObject>(damageEffectPrefab, _hitPoint);
        }

        if (IsAlive)
        {
            _anim?.SetTrigger(_hitTriggerHash);
        }
        else
        {
            _anim?.SetBool(_isAliveHash, false);
        }
    }
    #endregion IDamagable Interfaces
}
