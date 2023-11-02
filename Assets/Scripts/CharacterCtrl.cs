using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    public float _speed = 5f;
    public float _jumpHeight = 2f;
    public LayerMask _groundLayerMask;
    public float _groundCheckDistance = 0.3f;

    Animator _anim;
    Rigidbody _rb;
    Vector3 _inputDir = Vector3.zero;       // ������� �Է¿� ���� ���⼺�� ����ϴ� ����
    bool _isGrounded = false;
    bool _isJump = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        CheckGroundStatus();
        Jump();
    }

    void GetInput()
    {
        _inputDir = Vector3.zero;
        _inputDir.x = Input.GetAxis("Horizontal");
        _inputDir.z = Input.GetAxis("Vertical");

        if (_inputDir != Vector3.zero)
            transform.forward = _inputDir;
    }

    void Move()
    {
        _inputDir = new Vector3(_inputDir.x, 0, _inputDir.z).normalized;
        _rb.MovePosition(_rb.position + _inputDir * _speed * Time.deltaTime);
        _anim.SetBool("IsSprint", _inputDir != Vector3.zero);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !_isJump && _isGrounded)        // ���� ���
        {
            _rb.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
            _anim.SetBool("IsJump", _isGrounded = false);
            _isJump = true;
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;     // ����ĳ��Ʈ�� ���� �� �浹�� ������Ʈ�� ������ ������
#if UNITY_EDITOR
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * _groundCheckDistance));
#endif
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, _groundCheckDistance, _groundLayerMask))
            _isGrounded = true;

        else
            _isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _anim.SetBool("IsJump", false);
            _isJump = false;
        }
    }
}
