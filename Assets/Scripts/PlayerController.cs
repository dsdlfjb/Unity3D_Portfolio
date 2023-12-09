// 플레이어 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public string _currentMapName;      // Portal 스크립트에 있는 transferMapName 변수의 값을 저장

    Animator _anim;
    Camera _camera;
    NPCBattleUI _hpBar;
    Movement3D _movement3D;

    [SerializeField] float _smoothness = 10f;

    [SerializeField] bool _isRun;
    [SerializeField] bool _toggleCameraRotation;
    [SerializeField] public bool _isAttack;

    [SerializeField] Transform _cameraTransform;

    public int _maxHealth = 100;
    public int _health;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            _anim = this.GetComponent<Animator>();
            _camera = Camera.main;
            _hpBar = this.GetComponent<NPCBattleUI>();
            _movement3D = this.GetComponent<Movement3D>();

            instance = this;

            _health = _maxHealth;
        }

        else
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        AttackFalse();

        if (Input.GetKey(KeyCode.LeftAlt))
            _toggleCameraRotation = true;       // 둘러보기 활성화

        else
            _toggleCameraRotation = false;      // 둘러보기 비활성화

        if (Input.GetKey(KeyCode.LeftShift))
            _isRun = true;

        else
            _isRun = false;

        InputMovement();
        AttackTrue();
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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 애니메이션 파라미터 설정
        _anim.SetFloat("Horizontal", x);
        _anim.SetFloat("Vertical", z);

        // 이동 속도 설정 (앞으로 이동할 때만 5, 나머지는 2)
        _movement3D.MoveSpeed = z > 0 ? 5f : 2f;
        // 이동함수 호출 (카메라가 보고 있는 방향을 기준으로 방향키에 따라 이동)
        _movement3D.MoveTo(_cameraTransform.rotation * new Vector3(x, 0, z));
        // 회전 설정 (항상 앞만 보도록 캐릭터와 회전은 카메라와 같은 회전 값으로 설정)
        transform.rotation = Quaternion.Euler(0, _cameraTransform.eulerAngles.y, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _anim.SetTrigger("DoJump");
            _movement3D.JumpTo();       // 점프 함수 호출
        }
    }

    void AttackTrue()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _anim.SetTrigger("AttackTrigger");
            _isAttack = true;
        }
    }

    void AttackFalse() { _isAttack = false; }

    public void TakeDamage(int damageAmount)
    {
        _health -= damageAmount;
        _hpBar.Value -= damageAmount;
        _anim.SetTrigger("HitTrigger");

        if (_health <= 0)
            Die();
    }

    void Die()
    {
        _anim.SetTrigger("Die");
        //gameObject.SetActive(false);
    }
}