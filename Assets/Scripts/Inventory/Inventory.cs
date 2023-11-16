using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    PlayerController _player;

    InventorySlot[] _slots; // 인벤토리 슬롯들
    List<Item> _inventoryItemList;      // 플레이어가 소지한 아이템 리스트
    List<Item> _inventoryTabList;       // 선택한 탭에 따라 다르게 보여질 아이템 리스트

    public Text _descriptionText;       // 아이템 부연설명
    public string[] _tabDescription;

    public Transform _tf;       // 슬롯의 부모객체

    public GameObject _go;      // 인벤토리 활성화/비활성화
    public GameObject[] _selectedTabImages;

    int _selectedItem;       // 선택된 아이템
    int _selectedTab;

    bool _isActivated;      // 인벤토리 활성화
    bool _isTabActivated;       // 탭 활성화시 true
    bool _isItemActivated;       // 아이템 활성화시 true
    bool _stopKeyInput;     // 키 입력 제한
    bool _preventExec;      // 중복 실행 제한

    WaitForSeconds _waitTime = new WaitForSeconds(0.01f);

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _inventoryItemList = new List<Item>();
        _inventoryTabList = new List<Item>();
        _slots = _tf.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_stopKeyInput)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                _isActivated = !_isActivated;

                if (_isActivated)
                {
                    //_player.NotMove();
                    _go.SetActive(true);
                    _selectedTab = 0;
                    _isTabActivated = true;
                    _isItemActivated = false;
                    ShowTab();
                }

                else
                {
                    StopAllCoroutines();
                    _go.SetActive(false);
                    _isTabActivated = false;
                    _isItemActivated = false;
                    //_player.Move();
                }
            }

            if (_isActivated)
            {
                if (_isTabActivated)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (_selectedTab < _selectedTabImages.Length - 1)
                            _selectedTab++;

                        else
                            _selectedTab = 0;

                        SelectedTab();
                    }

                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (_selectedTab > 0)
                            _selectedTab--;

                        else
                            _selectedTab = _selectedTabImages.Length - 1;

                        SelectedTab();
                    }
                }
            }
        }
    }

    public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
    }

    public void RemoveSlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].RemoveItem();
            _slots[i].gameObject.SetActive(false);
        }
    }
    public void SelectedTab()
    {
        StopAllCoroutines();

        Color color = _selectedTabImages[_selectedTab].GetComponent<Image>().color;
        color.a = 0f;

        for (int i = 0; i < _selectedTabImages.Length; i++)
        {
            _selectedTabImages[i].GetComponent<Image>().color = color;
        }

        _descriptionText.text = _tabDescription[_selectedTab];
        StartCoroutine(Coroutine_SelectedTabEffect());
    }

    IEnumerator Coroutine_SelectedTabEffect()
    {
        while (_isTabActivated)
        {
            Color color = _selectedTabImages[0].GetComponent<Image>().color;

            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                _selectedTabImages[_selectedTab].GetComponent<Image>().color = color;
                yield return _waitTime;
            }

            while (color.a > 0f)
            {
                color.a -= 0.03f;
                _selectedTabImages[_selectedTab].GetComponent<Image>().color = color;
                yield return _waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
}
