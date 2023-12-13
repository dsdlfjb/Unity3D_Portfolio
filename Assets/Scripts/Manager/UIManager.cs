using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI _lvText;
    public Image _hpBar;
    public Image _mpBar;
    public Slider _expBar;

    public ItemDataBase _itemDatabase;

    public StaticInventoryUI _equipmentUI;
    public DynamicInventoryUI _inventoryUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _hpBar.fillAmount = 1;
        _mpBar.fillAmount = 1;
        _expBar.value = 0;
    }

    private void Update()
    {
        ShowInventoryUI();
        ShowEquipmentUI();
        Level_UP();
    }

    void ShowInventoryUI()
    {
        if (Input.GetKeyDown("i"))
        {
            _inventoryUI.gameObject.SetActive(!_inventoryUI.gameObject.activeSelf);
        }
    }

    void ShowEquipmentUI()
    {
        if (Input.GetKeyDown("e"))
        {
            _equipmentUI.gameObject.SetActive(!_equipmentUI.gameObject.activeSelf);
        }
    }

    // 경험치가 다 차면 레벨업과 최대 경험치 증가
    public void EXP_UP()
    {
        float curExp = GameManager.Instance._exp;
        float maxExp = GameManager.Instance._nextExp[Mathf.Min(GameManager.Instance._level, GameManager.Instance._nextExp.Length - 1)];
        _expBar.value = curExp / maxExp;
    }

    // 레벨업 함수
    public void Level_UP()
    {
        _lvText.text = string.Format("{0:F0}", GameManager.Instance._level);
    }
}
