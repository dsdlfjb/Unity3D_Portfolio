// ĳ���Ͱ� ����ȯ�� ���� �� ���۵Ǵ� ������ ���ϴ� ��ũ��Ʈ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string _startPoint;      // �÷��̾ ���۵� ��ġ

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
