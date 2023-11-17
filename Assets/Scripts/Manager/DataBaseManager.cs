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


    // Start is called before the first frame update
    void Start()
    {

    }
}
