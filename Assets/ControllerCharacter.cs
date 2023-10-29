using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllerCharacter : MonoBehaviour
{
    #region Variables
    private CharacterController _characterController;
    private Vector3 _calcVelocity = Vector3.zero;
    private bool _isGrounded = false;

    NavMeshAgent _agent;
    Camera _camera;

    [SerializeField]
    private float _groundCheckDistance = 0.2f;
    [SerializeField]
    private LayerMask _groundLayerMask;


    #endregion

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _agent.updatePosition = false;
        _agent.updateRotation = true;
        _camera = Camera.main;
    }

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

            if (_agent.remainingDistance > _agent.stoppingDistance)
            {
                _characterController.Move(_agent.velocity * Time.deltaTime);
            }
            else
            {
                _characterController.Move(Vector3.zero);
            }
        }
    }

    private void LateUpdate()
    {
        transform.position = _agent.nextPosition;
    }
}