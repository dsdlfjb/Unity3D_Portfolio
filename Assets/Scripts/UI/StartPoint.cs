// 캐릭터가 씬전환을 했을 때 시작되는 지점을 정하는 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string _startPoint;      // 플레이어가 시작될 위치

    PlayerController _player;
    CameraMovement _camera;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _camera = FindObjectOfType<CameraMovement>();

        if (_startPoint == _player._currentMapName)
        {
            _camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            _player.transform.position = this.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
