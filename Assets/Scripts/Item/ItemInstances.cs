// Combiner���� ������ �����۵��� ����Ʈ�� �����ϴ� ��ũ��Ʈ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstances : MonoBehaviour
{
    public List<Transform> _items = new List<Transform>();

    private void OnDestroy()
    {
        foreach (Transform item in _items)
        {
            Destroy(item.gameObject);
        }
    }
}
