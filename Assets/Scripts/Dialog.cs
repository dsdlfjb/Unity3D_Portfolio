using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [Tooltip("��� ġ�� ĳ���� �̸�")]
    public string _name;

    [Tooltip("��� ����")]
    public string[] _contexts;
}
[System.Serializable]
public class DialogEvent
{
    public string name;

    public Vector2 line;
    public Dialog[] dialogs;
}
