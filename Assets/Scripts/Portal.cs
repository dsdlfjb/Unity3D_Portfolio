// �÷��̾ �ٸ� ������ �ڷ���Ʈ ��Ű�� ��Ż�� ���� ��ũ��Ʈ
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
