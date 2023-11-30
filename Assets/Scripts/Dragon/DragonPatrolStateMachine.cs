using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragonPatrolStateMachine : StateMachineBehaviour
{
    float _timer;
    List<Transform> _waypoints = new List<Transform>();
    NavMeshAgent _agent;
    Transform _player;
    float _chaseRange = 8;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = animator.GetComponent<NavMeshAgent>();
        _agent.speed = 1.5f;
        _timer = 0;
        GameObject go = GameObject.FindGameObjectWithTag("Waypoint");

        foreach (Transform t in go.transform)
            _waypoints.Add(t);

        _agent.SetDestination(_waypoints[Random.Range(0, _waypoints.Count)].position);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance)
            _agent.SetDestination(_waypoints[Random.Range(0, _waypoints.Count)].position);

        _timer += Time.deltaTime;

        if (_timer > 10)
            animator.SetBool("IsPatrolling", false);

        float distance = Vector3.Distance(_player.position, animator.transform.position);

        if (distance < _chaseRange)
            animator.SetBool("IsChasing", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(_agent.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
