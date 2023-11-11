using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    TextMeshProUGUI _tmp;

    public float _destroyDelayTime = 1f;

    public int Damage
    {
        get
        {
            if (_tmp != null)
                return int.Parse(_tmp.text);

            return 0;
        }

        set
        {
            if (_tmp != null)
                _tmp.text = value.ToString();
        }
    }

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _destroyDelayTime);
    }
}
