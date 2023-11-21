// Combiner에서 생성된 아이템들의 리스트를 관리하는 스크립트
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
