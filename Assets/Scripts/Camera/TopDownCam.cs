// ž�ٿ� ī�޶� ����
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SYC
{
    public class TopDownCam : MonoBehaviour
    {
        public float _height = 5f;      // ī�޶� ����
        public float _distance = 10f;        // ī�޶�� Ÿ���� �Ÿ�
        public float _angle = 45f;      // ī�޶��� ����
        public float _lookAtHeight = 2f;        // ī�޶�� ��ǥ Ÿ���� ����
        public float _smoothSpeed = 0.5f;      // ī�޶� �̵��� �� �ε巴�� �ϱ� ���� ����


        Vector3 _refVelocity;       // ���ΰ����� ���� ����

        public Transform _target;

        // ī�޶� �̵��� �� ���� �� �ڿ������� �̵��� �� �ֵ��� LateUpdate �Լ��� ���
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
