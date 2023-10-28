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
    Vector3 _inputDir = Vector3.zero;       // 사용자의 입력에 대한 방향성을 계산하는 변수
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

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)        // 점프 기능
        {
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y);
            _rb.AddForce(jumpVelocity, ForceMode.VelocityChange);     // AddForce 하는 이유 : 물리엔지닝에 의해서 입련된 velocity가 자동으로 계산됨
                                                                                                    // VelocityChange - 가속도를 변경하는 것으로 설정
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))    //  대시 기능
        {
            // 현재 설정된 리지드바디에 저항값을 로그 함수화 시켜서 점차적으로 자연스럽게 멈출 수 있도록 설정
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
