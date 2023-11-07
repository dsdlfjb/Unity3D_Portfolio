// 플레이어를 다른 씬으로 텔레포트 시키는 포탈에 관한 스크립트
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string _transferMapName;

    PlayerController _player;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player._currentMapName = _transferMapName;
            StartCoroutine(Coroutine_ChangeScene());
        }
    }

    IEnumerator Coroutine_ChangeScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(_transferMapName);
    }
}
