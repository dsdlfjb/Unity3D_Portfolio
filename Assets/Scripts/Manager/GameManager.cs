using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("[ �÷��̾� ���� ]")]
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
            _level++;
            _exp = 0;
        }
    }

    // ������ �ٽ� �����ϴ� �Լ� �ۼ�

    // ���� ������ �̰�ٴ� �Լ� �ۼ�
}