using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public ItemDataBase _itemDatabase;

    public StaticInventoryUI _equipmentUI;
    public DynamicInventoryUI _inventoryUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            _inventoryUI.gameObject.SetActive(!_inventoryUI.gameObject.activeSelf);
        }

        if (Input.GetKeyDown("e"))
        {
            _equipmentUI.gameObject.SetActive(!_equipmentUI.gameObject.activeSelf);
        }
    }
}
