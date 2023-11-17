using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    PlayerController _player;

    InventorySlot[] _slots; // �κ��丮 ���Ե�
    List<Item> _inventoryItemList;      // �÷��̾ ������ ������ ����Ʈ
    List<Item> _inventoryTabList;       // ������ �ǿ� ���� �ٸ��� ������ ������ ����Ʈ

    public Text _descriptionText;       // ������ �ο�����
    public string[] _tabDescription;

    public Transform _tf;       // ������ �θ�ü

    public GameObject _go;      // �κ��丮 Ȱ��ȭ/��Ȱ��ȭ
    public GameObject[] _selectedTabImages;

    int _selectedItem;       // ���õ� ������
    int _selectedTab;

    bool _isActivated;      // �κ��丮 Ȱ��ȭ
    bool _isTabActivated;       // �� Ȱ��ȭ�� true
    bool _isItemActivated;       // ������ Ȱ��ȭ�� true
    bool _stopKeyInput;     // Ű �Է� ����
    bool _preventExec;      // �ߺ� ���� ����

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
            } // �κ��丮 ���� �ݱ�

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

                } // �� Ȱ��ȭ�� Ű�Է� ó��.

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
                            if (_selectedTab == 0) // �Ҹ�ǰ
                            {
                                _stopKeyInput = true;
                                // ������ ���� �ų�? ���� ������ ȣ��
                            }

                            else if (_selectedTab == 1)
                            {
                                // ��� ����
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
                } // ������ Ȱ��ȭ�� Ű�Է� ó��.

                if (Input.GetKeyUp(KeyCode.Z)) // �ߺ� ���� ����.
                    _preventExec = false;
            } // �κ��丮�� ������ Ű�Է�ó�� Ȱ��ȭ.
        }
    }

    // �� Ȱ��ȭ
    public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
    }

    // �κ��丮 ���� �ʱ�ȭ
    public void RemoveSlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].RemoveItem();
            _slots[i].gameObject.SetActive(false);
        }
    }

    // ���õ� ���� �����ϰ� �ٸ� ��� ���� �÷� ���İ� 0���� ����
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

    // ���õ� �� ��¦�� ȿ��
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

    // ������ Ȱ��ȭ (inventoryTabList�� ���ǿ� �´� �����۵鸸 �־��ְ�, �κ��丮 ���Կ� ���)
    public void ShowItem()
    {
        _inventoryTabList.Clear();
        RemoveSlot();
        _selectedItem = 0;

        // �ǿ� ���� ������ �з�. �װ��� �κ��丮 �� ����Ʈ�� �߰�
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

        // �κ��丮 �� ����Ʈ�� ������ �κ��丮 ���Կ� �߰�
        for (int i = 0; i < _inventoryTabList.Count; i++)
        {
            _slots[i].gameObject.SetActive(true);
            _slots[i].AddItem(_inventoryTabList[i]);
        }

        SelectedItem();
    }

    // ���õ� �������� �����ϰ� �ٸ� ��� ���� �÷� ���İ��� 0���� ����
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
            _descriptionText.text = "�ش� Ÿ���� �������� �����ϰ� ���� �ʽ��ϴ�.";
    }

    // ���õ� ������ ��¦�� ȿ��
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
