// 탑다운 카메라 설정
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SYC
{
    public class TopDownCam : MonoBehaviour
    {
        public float _height = 5f;      // 카메라 높이
        public float _distance = 10f;        // 카메라와 타겟의 거리
        public float _angle = 45f;      // 카메라의 각도
        public float _lookAtHeight = 2f;        // 카메라와 목표 타겟의 높이
        public float _smoothSpeed = 0.5f;      // 카메라가 이동할 때 부드럽게 하기 위한 변스


        Vector3 _refVelocity;       // 내부개선을 위한 변수

        public Transform _target;

        // 카메라가 이동할 때 조금 더 자연스럽게 이동할 수 있도록 LateUpdate 함수를 사용
        private void LateUpdate()
        {
            HandleCamera();
        }

        public void HandleCamera()
        {
            if (!_target) return;

            Vector3 worldPosition = (Vector3.forward * -_distance) + (Vector3.up * _height);

            Vector3 rotatedVec = Quaternion.AngleAxis(_angle, Vector3.up) * worldPosition;

            Vector3 finalTargetPos = _target.position;
            finalTargetPos.y += _lookAtHeight;

            Vector3 finalPos = finalTargetPos + rotatedVec;

            transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref _refVelocity, _smoothSpeed);

            transform.LookAt(_target.position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);

            if (_target)
            {
                Vector3 lookAtPos = _target.position;
                lookAtPos.y += _lookAtHeight;
                Gizmos.DrawLine(transform.position, lookAtPos);
                Gizmos.DrawSphere(lookAtPos, 0.25f);
            }

            Gizmos.DrawSphere(transform.position, 0.25f);
        }
    }
}
