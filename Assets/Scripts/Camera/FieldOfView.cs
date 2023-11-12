using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("Sight Settings")]
    public float _viewRaidius = 5f;
    [Range(0, 360)]
    public float _viewAngle = 90f;

    [Header("Find Settings")]
    public float _delay = 0.2f;

    public LayerMask _targetMask;
    public LayerMask _obstacleMask;

    List<Transform> _visibleTargets = new List<Transform>();
    Transform _nearestTarget;

    float _distanceToTarget = 0f;

    public List<Transform> VisibleTargets => _visibleTargets;
    public Transform NearestTarget => _nearestTarget;
    public float DistanceToTarget => _distanceToTarget;

    private void Start()
    {
        StartCoroutine(Coroutine_FindTargetsWithDelay(_delay));    
    }

    private void Update()
    {
        FindVisibleTargets();
    }

    IEnumerator Coroutine_FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        _distanceToTarget = 0f;
        _nearestTarget = null;
        _visibleTargets.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, _viewRaidius, _targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < _viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, _obstacleMask))
                {
                    if (target.GetComponent<IDamagable>()?.IsAlive ?? false)
                    {
                        _visibleTargets.Add(target);

                        if (_nearestTarget == null || (_distanceToTarget > distToTarget))
                            _nearestTarget = target;

                        _distanceToTarget = distToTarget;
                    }
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
