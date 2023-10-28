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
    public Vector3 _drags;      // 저항값

    CharacterController _characterController;
    Vector3 _inputDir = Vector3.zero;       // 사용자의 입력에 대한 방향성을 계산하는 변수
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

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)        // 점프 기능
        {
            _calVelocity.y += Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))    //  대시 기능
        {
            // 현재 설정된 리지드바디에 저항값을 로그 함수화 시켜서 점차적으로 자연스럽게 멈출 수 있도록 설정
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
        RaycastHit hitInfo;     // 레이캐스트를 했을 때 충돌된 오브젝트의 정보를 가져옴
#if UNITY_EDITOR
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * _groundCheckDistance));
#endif
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, _groundCheckDistance, _groundLayerMask))
            _isGrounded = true;

        else
            _isGrounded = false;
    }
}
