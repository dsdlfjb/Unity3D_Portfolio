using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    Vector3 _moveDir;
    CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _controller.Move(_moveDir * _moveSpeed * Time.deltaTime);
    }

    public void MoveTo(Vector3 dir)
    {
        _moveDir = dir;
    }
}
