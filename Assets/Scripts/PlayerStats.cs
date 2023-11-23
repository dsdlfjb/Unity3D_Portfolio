using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int _lv;
    public int[] _needExp;
    public int _curExp;

    public int _hp;
    public int _curHp;
    public int _mp;
    public int _curMp;

    public int _atk;
    public int _def;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
