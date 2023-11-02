using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField]
    Transform _player;

    private void LateUpdate()
    {
        Vector3 newPos = _player.position;
        newPos.y = transform.position.y;
        transform.position = newPos;

        transform.rotation = Quaternion.Euler(90f, _player.eulerAngles.y, 0f);
    }
}
