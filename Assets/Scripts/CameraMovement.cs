// 마우스로 카메라 회전을 위한 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform _objectFollow;
    [SerializeField] float _followSpeed = 10f;
    [SerializeField] float _sensitivity = 100f;
    [SerializeField] float _clampAngle = 70f;

    [SerializeField] Transform _mainCamera;
    [SerializeField] Vector3 _dirNormalized;
    [SerializeField] Vector3 _finalDir;
    [SerializeField] float _minDist;
    [SerializeField] float _maxDist;
    [SerializeField] float _finalDist;
    [SerializeField] float _smoothness = 10f;

    float _rotX;
    float _rotY;

    // Start is called before the first frame update
    void Start()
    {
        _rotX = transform.localRotation.eulerAngles.x;
        _rotX = transform.localRotation.eulerAngles.y;

        _dirNormalized = _mainCamera.localPosition.normalized;
        _finalDist = _mainCamera.localPosition.magnitude;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        _rotX += -(Input.GetAxis("Mouse Y")) * _sensitivity * Time.deltaTime;
        _rotY += Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;

        _rotX = Mathf.Clamp(_rotX, -_clampAngle, _clampAngle);
        Quaternion rot = Quaternion.Euler(_rotX, _rotY, 0);
        transform.rotation = rot;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _objectFollow.position, _followSpeed * Time.deltaTime);

        _finalDir = transform.TransformPoint(_dirNormalized * _maxDist);

        RaycastHit hit;

        if (Physics.Linecast(transform.position, _finalDir, out hit))
            _finalDist = Mathf.Clamp(hit.distance, _minDist, _maxDist);

        else
            _finalDist = _maxDist;

        _mainCamera.localPosition = Vector3.Lerp(_mainCamera.localPosition, _dirNormalized * _finalDist, Time.deltaTime * _smoothness);
    }
}
