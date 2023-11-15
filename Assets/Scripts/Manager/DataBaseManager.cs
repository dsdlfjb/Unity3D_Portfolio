using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
    }

    public string[] _varName;
    public float[] _var;

    public string[] _switchName;
    public bool[] _switches;

    public List<Item> _itemList = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        _itemList.Add(new Item(10001, "HP포션", "체력을 50 회복시켜주는 마법의 물약", Item.EItemType.Use));
    }
}
