using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCharacter : MonoBehaviour
{
    public float _speed = 5f;
    public float _jumpHeight = 2f;
    public float _dashDistance = 5f;
    public LayerMask _groundLayerMask;
    public float _groundCheckDistance = 0.3f;
    public float _gravity = -9.81f;
    public Vector3 _drags;      // ���װ�

    CharacterController _characterController;
    Vector3 _inputDir = Vector3.zero;       // ������� �Է¿� ���� ���⼺�� ����ϴ� ����
    bool _isGrounded = false;
    Vector3 _calVelocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = _characterController.isGrounded;

        if (_isGrounded && _calVelocity.y < 0)
            _calVelocity.y = 0;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _characterController.Move(move * Time.deltaTime * _speed);

        if (move != Vector3.zero)
        {
            transform.forward = _inputDir;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)        // ���� ���
        {
            _calVelocity.y += Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))    //  ��� ���
        {
            // ���� ������ ������ٵ� ���װ��� �α� �Լ�ȭ ���Ѽ� ���������� �ڿ������� ���� �� �ֵ��� ����
            Vector3 dashVelocity = Vector3.Scale(transform.forward,
                                                                    _dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _drags.x + 1)) / -Time.deltaTime),
                                                                   0,
                                                                   (Mathf.Log(1f / (Time.deltaTime * _drags.z + 1)) / -Time.deltaTime)));

            _calVelocity += dashVelocity;
        }

        _calVelocity.y += _gravity * Time.deltaTime;

        _calVelocity.x /= 1 + _drags.x * Time.deltaTime;
        _calVelocity.y /= 1 + _drags.y * Time.deltaTime;
        _calVelocity.z /= 1 + _drags.z * Time.deltaTime;

        _characterController.Move(_calVelocity * Time.deltaTime);
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
