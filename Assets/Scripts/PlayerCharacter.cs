using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(CharacterController)), RequireComponent(typeof(Animator))]
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    LayerMask _groundLayerMask;
    [SerializeField]
    Animator _anim;

    CharacterController _controller;
    NavMeshAgent _agent;
    Camera _camera;

    readonly int _moveHash = Animator.StringToHash("IsWalk");
    readonly int _fallingHash = Animator.StringToHash("IsFall");

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        _agent.updateRotation = true;
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, _groundLayerMask))
            {
                Debug.Log("We hit " + hit.collider.name + " " + hit.point);
                _agent.SetDestination(hit.point);
            }
        }

        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            _controller.Move(_agent.velocity * Time.deltaTime);
            _anim.SetBool(_moveHash, true);
        }

        else
        {
            _controller.Move(Vector3.zero);
            _anim.SetBool(_moveHash, false);
        }

        if (_agent.isOnOffMeshLink)
            _anim.SetBool(_fallingHash, _agent.velocity.y != 0.0f);

        else
            _anim.SetBool(_fallingHash, false);
    }

    private void OnAnimatorMove()
    {
        Vector3 position = _agent.nextPosition;
        _anim.rootPosition = _agent.nextPosition;
        transform.position = position;
    }
}
