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
            } // 인벤토리 열고 닫기

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
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (_selectedTab > 0)
                            _selectedTab--;
                        else
                            _selectedTab = _selectedTabImages.Length - 1;

                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        Color color = _selectedTabImages[_selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        _selectedTabImages[_selectedTab].GetComponent<Image>().color = color;
                        _isItemActivated = true;
                        _isTabActivated = false;
                        _preventExec = true;
                        ShowItem();
                    }

                } // 탭 활성화시 키입력 처리.

                else if (_isItemActivated)
                {
                    if (_inventoryTabList.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            if (_selectedItem < _inventoryTabList.Count - 2)
                                _selectedItem += 2;
                            else
                                _selectedItem %= 2;
                            
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            if (_selectedItem > 1)
                                _selectedItem -= 2;
                            else
                                _selectedItem = _inventoryTabList.Count - 1 - _selectedItem;

                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (_selectedItem < _inventoryTabList.Count - 1)
                                _selectedItem++;
                            else
                                _selectedItem = 0;
                            
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (_selectedItem > 0)
                                _selectedItem--;
                            else
                                _selectedItem = _inventoryTabList.Count - 1;

                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !_preventExec)
                        {
                            if (_selectedTab == 0) // 소모품
                            {
                                _stopKeyInput = true;
                                // 물약을 마식 거냐? 같은 선택지 호출
                            }

                            else if (_selectedTab == 1)
                            {
                                // 장비 장착
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        StopAllCoroutines();
                        _isItemActivated = false;
                        _isTabActivated = true;
                        ShowTab();
                    }
                } // 아이템 활성화시 키입력 처리.

                if (Input.GetKeyUp(KeyCode.Z)) // 중복 실행 방지.
                    _preventExec = false;
            } // 인벤토리가 열리면 키입력처리 활성화.
        }
    }

    // 탭 활성화
    public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
    }

    // 인벤토리 슬롯 초기화
    public void RemoveSlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].RemoveItem();
            _slots[i].gameObject.SetActive(false);
        }
    }

    // 선택된 탭을 제외하고 다른 모든 탭의 컬러 알파값 0으로 조정
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

    // 선택된 탭 반짝임 효과
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

    // 아이템 활성화 (inventoryTabList에 조건에 맞는 아이템들만 넣어주고, 인벤토리 슬롯에 출력)
    public void ShowItem()
    {
        _inventoryTabList.Clear();
        RemoveSlot();
        _selectedItem = 0;

        // 탭에 따른 아이템 분류. 그것을 인벤토리 탭 리스트에 추가
        switch(_selectedTab)       
        {
            case 0:
                for (int i = 0; i < _inventoryItemList.Count; i++)
                {
                    if (Item.EItemType.Use == _inventoryItemList[i]._eItemType)
                        _inventoryTabList.Add(_inventoryItemList[i]);
                }
                break;

            case 1:
                for (int i = 0; i < _inventoryItemList.Count; i++)
                {
                    if (Item.EItemType.Equip == _inventoryItemList[i]._eItemType)
                        _inventoryTabList.Add(_inventoryItemList[i]);
                }
                break;

            case 2:
                for (int i = 0; i < _inventoryItemList.Count; i++)
                {
                    if (Item.EItemType.Quest == _inventoryItemList[i]._eItemType)
                        _inventoryTabList.Add(_inventoryItemList[i]);
                }
                break;

            case 3:
                for (int i = 0; i < _inventoryItemList.Count; i++)
                {
                    if (Item.EItemType.Etc == _inventoryItemList[i]._eItemType)
                        _inventoryTabList.Add(_inventoryItemList[i]);
                }
                break;
        }

        // 인벤토리 탭 리스트에 내용을 인벤토리 슬롯에 추가
        for (int i = 0; i < _inventoryTabList.Count; i++)
        {
            _slots[i].gameObject.SetActive(true);
            _slots[i].AddItem(_inventoryTabList[i]);
        }

        SelectedItem();
    }

    // 선택된 아이템을 제외하고 다른 모든 탭의 컬러 알파값을 0으로 조정
    public void SelectedItem()
    {
        StopAllCoroutines();

        if (_inventoryTabList.Count > 0)
        {
            Color color = _slots[0]._selectedItem.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i < _inventoryTabList.Count; i++)
                _slots[i]._selectedItem.GetComponent<Image>().color = color;

            _descriptionText.text = _inventoryTabList[_selectedItem]._itemDescription;
            StartCoroutine(Coroutine_SelectedItemEffect());
        }

        else
            _descriptionText.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
    }

    // 선택된 아이템 반짝임 효과
    IEnumerator Coroutine_SelectedItemEffect()
    {
        while (_isItemActivated)
        {
            Color color = _slots[0].GetComponent<Image>().color;

            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                _slots[_selectedItem]._selectedItem.GetComponent<Image>().color = color;
                yield return _waitTime;
            }

            while (color.a > 0f)
            {
                color.a -= 0.03f;
                _slots[_selectedItem]._selectedItem.GetComponent<Image>().color = color;
                yield return _waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
}
