// �÷��̾� ��ũ��Ʈ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public string _currentMapName;      // Portal ��ũ��Ʈ�� �ִ� transferMapName ������ ���� ����

    Animator _anim;
    Camera _camera;
    NPCBattleUI _hpBar;
    Movement3D _movement3D;

    [SerializeField] float _smoothness = 10f;

    [SerializeField] bool _isRun;
    [SerializeField] bool _toggleCameraRotation;
    [SerializeField] public bool _isAttack;

    [SerializeField] Transform _cameraTransform;
    
    public GameObject _currentWeaponInHand;

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
            _toggleCameraRotation = true;       // �ѷ����� Ȱ��ȭ

        else
            _toggleCameraRotation = false;      // �ѷ����� ��Ȱ��ȭ

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

        // �ִϸ��̼� �Ķ���� ����
        _anim.SetFloat("Horizontal", x);
        _anim.SetFloat("Vertical", z);

        // �̵� �ӵ� ���� (������ �̵��� ���� 5, �������� 2)
        _movement3D.MoveSpeed = z > 0 ? 5f : 2f;
        // �̵��Լ� ȣ�� (ī�޶� ���� �ִ� ������ �������� ����Ű�� ���� �̵�)
        _movement3D.MoveTo(_cameraTransform.rotation * new Vector3(x, 0, z));
        // ȸ�� ���� (�׻� �ո� ������ ĳ���Ϳ� ȸ���� ī�޶�� ���� ȸ�� ������ ����)
        transform.rotation = Quaternion.Euler(0, _cameraTransform.eulerAngles.y, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _anim.SetTrigger("DoJump");
            _movement3D.JumpTo();       // ���� �Լ� ȣ��
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
        if (_hpBar != null)
        {
            _hpBar.Value -= damageAmount;
        }
        _anim.SetTrigger("HitTigger");
        Debug.Log($"cur Player HP {_health}");

        if (_health <= 0)
            Die();
    }
    
    

    void Die()
    {
        _anim.SetTrigger("Die");
        //gameObject.SetActive(false);
    }
    
    public void StartDealDamage()
    {
        _currentWeaponInHand.transform.GetChild(0).GetComponentInChildren<PlayerDamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        _currentWeaponInHand.transform.GetChild(0).GetComponentInChildren<PlayerDamageDealer>().StartDealDamage();
    }
}