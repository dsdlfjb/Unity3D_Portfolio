using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCBattleUI : MonoBehaviour
{
    Slider _hpSlider;

    [SerializeField]
    GameObject _damageText;

    public float MinimumValue
    {
        get => _hpSlider.minValue;
        set { _hpSlider.minValue = value; }
    }

    public float MaximumValue
    {
        get => _hpSlider.maxValue;
        set { _hpSlider.maxValue = value; }
    }

    public float Value
    {
        get => _hpSlider.value;
        set { _hpSlider.value = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _hpSlider = gameObject.GetComponentInChildren<Slider>();
    }

    private void OnEnable()
    {
        GetComponent<Canvas>().enabled = true;
    }

    private void OnDisable()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void TakeDamage(int damage)
    {
        if (_damageText != null)
        {
            GameObject damageTextGO = Instantiate(_damageText, transform);
            DamageText damageText = damageTextGO.GetComponent<DamageText>();

            if (damageText == null)
                Destroy(damageTextGO);

            damageText.Damage = damage;
        }
    }
}
