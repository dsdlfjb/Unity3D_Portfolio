using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    public static CopyPosition instance;

    [SerializeField]
    Transform _player;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
            
        else
            Destroy(this.gameObject);
    }

    
    private void LateUpdate()
    {
        Vector3 newPos = _player.position;
        newPos.y = transform.position.y;
        transform.position = newPos;

        transform.rotation = Quaternion.Euler(90f, _player.eulerAngles.y, 0f);
    }
}
