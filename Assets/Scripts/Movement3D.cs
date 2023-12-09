using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _jumpForce = 3f;

    Vector3 _moveDirection;     // 이동 방향
    CharacterController _controller;

    public float MoveSpeed
    {
        set => _moveSpeed = Mathf.Clamp(value, 2f, 5f);
    }
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 중력 설정, 플레이어가 땅을 밟고 있지 않다면
        // y축 이동방향에 gravity * Time.deltaTime을 더해줌
        if (_controller.isGrounded == false)
            _moveDirection.y += _gravity * Time.deltaTime;

        // 이동 설정, Character Controller의 Move() 함수를 이용한 이동
        _controller.Move(_moveDirection * _moveSpeed * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        _moveDirection = new Vector3(direction.x, _moveDirection.y, direction.z);
    }

    public void JumpTo()
    {
        // 캐릭터가 바닥을 밟고 있으면 점프
        if (_controller.isGrounded == true)
            _moveDirection.y = _jumpForce;
    }
}
