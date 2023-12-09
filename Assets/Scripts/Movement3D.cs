using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _jumpForce = 3f;

    Vector3 _moveDirection;     // �̵� ����
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
        // �߷� ����, �÷��̾ ���� ��� ���� �ʴٸ�
        // y�� �̵����⿡ gravity * Time.deltaTime�� ������
        if (_controller.isGrounded == false)
            _moveDirection.y += _gravity * Time.deltaTime;

        // �̵� ����, Character Controller�� Move() �Լ��� �̿��� �̵�
        _controller.Move(_moveDirection * _moveSpeed * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        _moveDirection = new Vector3(direction.x, _moveDirection.y, direction.z);
    }

    public void JumpTo()
    {
        // ĳ���Ͱ� �ٴ��� ��� ������ ����
        if (_controller.isGrounded == true)
            _moveDirection.y = _jumpForce;
    }
}
