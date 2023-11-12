using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacing : MonoBehaviour
{
	Camera referenceCamera;

	public enum EAxis { up, down, left, right, forward, back };
	public bool _reverseFace = false;
	public EAxis _axis = EAxis.up;

	public Vector3 GetAxis(EAxis refAxis)
	{
		switch (refAxis)
		{
			case EAxis.down:
				return Vector3.down;
			case EAxis.forward:
				return Vector3.forward;
			case EAxis.back:
				return Vector3.back;
			case EAxis.left:
				return Vector3.left;
			case EAxis.right:
				return Vector3.right;
		}

		return Vector3.up;
	}

	void Awake()
	{
		if (!referenceCamera)
			referenceCamera = Camera.main;
	}

	void LateUpdate()
	{
		Vector3 targetPos = transform.position + referenceCamera.transform.rotation * (_reverseFace ? Vector3.forward : Vector3.back);
		Vector3 targetOrientation = referenceCamera.transform.rotation * GetAxis(_axis);
		transform.LookAt(targetPos, targetOrientation);
	}
}