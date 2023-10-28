using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyCharacter : MonoBehaviour
{
    public float _speed = 5f;
    public float _jumpHeight = 2f;
    public float _dashDistance = 5f;
    public LayerMask _groundLayerMask;
    public float _groundCheckDistance = 0.3f;

    Rigidbody _rb;
    Vector3 _inputDir = Vector3.zero;       // ������� �Է¿� ���� ���⼺�� ����ϴ� ����
    bool _isGrounded = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGroundStatus();

        _inputDir = Vector3.zero;
        _inputDir.x = Input.GetAxis("Horizontal");
        _inputDir.z = Input.GetAxis("Vertical");

        if (_inputDir != Vector3.zero)
            transform.forward = _inputDir;

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)        // ���� ���
        {
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y);
            _rb.AddForce(jumpVelocity, ForceMode.VelocityChange);     // AddForce �ϴ� ���� : ���������׿� ���ؼ� �Էõ� velocity�� �ڵ����� ����
                                                                                                    // VelocityChange - ���ӵ��� �����ϴ� ������ ����
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))    //  ��� ���
        {
            // ���� ������ ������ٵ� ���װ��� �α� �Լ�ȭ ���Ѽ� ���������� �ڿ������� ���� �� �ֵ��� ����
            Vector3 dashVelocity = Vector3.Scale(transform.forward, 
                                                                    _dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _rb.drag + 1)) / -Time.deltaTime),
                                                                   0,
                                                                   (Mathf.Log(1f / (Time.deltaTime * _rb.drag + 1)) / -Time.deltaTime)));

            _rb.AddForce(dashVelocity, ForceMode.VelocityChange);
        }
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _inputDir * _speed * Time.fixedDeltaTime);
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
}
