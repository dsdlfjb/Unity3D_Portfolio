using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("[ 플레이어 정보 ]")]
    public int _level;
    public int _exp;
    public int[] _nextExp = { };

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != null) return;
        DontDestroyOnLoad(gameObject);
    }

    public void GetExp()
    {
        _exp++;

        if (_exp == _nextExp[Mathf.Min(_level, _nextExp.Length - 1)])
        {
            // [KMONG] 스킬포인트 변수 사용
            PlayerController.instance._skillPoint += 5;
            _level += 5;
            _exp = 0;
        }
    }

    // 죽으면 다시 시작하는 함수 작성

    // 보스 잡으면 이겼다는 함수 작성
}
