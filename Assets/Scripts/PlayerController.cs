using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator _anim;
    Camera _camera;
    CharacterController _controller;

    [SerializeField] float _speed = 5f;
    [SerializeField] float _runSpeed = 8f;
    [SerializeField] float _finalSpeed;
    [SerializeField] float _smoothness = 10f;
    [SerializeField] bool _toggleCameraRotation;
    [SerializeField] bool _isRun;

    private void Awake()
    {
        _anim = this.GetComponent<Animator>();
        _camera = Camera.main;
        _controller = this.GetComponent<CharacterController>();
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

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDir = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        _controller.Move(moveDir.normalized * _finalSpeed * Time.deltaTime);

        float percent = ((_isRun) ? 1 : 0.5f) * moveDir.magnitude;
        _anim.SetFloat("Speed", percent, 0.1f, Time.deltaTime);     // dampTime : 0.1안에 즉각적인 반응?

    }
}
