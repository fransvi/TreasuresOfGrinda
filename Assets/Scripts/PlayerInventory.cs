using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    [SerializeField]
    private GameObject[] _mWeapons;
    [SerializeField]
    private GameObject[] _oWeapons;

    [SerializeField]
    private GameObject _activeMainWeapon;
    private bool _hasKey1;
    private float _playerGold;
    private bool _hasHealthPotion;
    private GameObject _activeOffWeapon;
    private GameObject _playerController;

    [SerializeField]
    private int _currentMainWeaponInt;

    private int _currentOffWeaponInt;

    /*

    Item types:

    0: Consumable
        0: Health potion
        1: Large Coin
        2: Small Coin
        3: Key1
    1: Main Weapon
        0: Sword
        1: Mace
    2: Off Weapon
        0: None
        1: Bow
        2: Bomb
        3: Shield


    */


    private bool _hasBow;

	// Use this for initialization
	void Start () {
        _hasKey1 = false;
        _hasHealthPotion = false;
        _hasBow = false;
    }

    public void SetPlayer(GameObject plr)
    {
        _playerController = plr;
    }


    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public int GetCurrentMainWeapon()
    {
        return _currentMainWeaponInt;
    }

    public void SetCurrentMainWeapon(int ne)
    {
        _currentMainWeaponInt = ne;
    }

    public int GetCurrentOffWeapon()
    {
        return _currentOffWeaponInt;
    }
    public void SetCurrentOffWeapon(int ne)
    {
        _currentOffWeaponInt = ne;
    }


    public void gainGold(float val)
    {
        this._playerGold += val;
    }

    public void loseGold(float val)
    {
        if(_playerGold - val >= 0)
        {
            _playerGold -= val;
        }
    }

    public float GetCurrentGold()
    {
        return _playerGold;
    }
    public void SetCurrentGold(float f)
    {
        _playerGold = f;
    }
    void OnGUI()
    {
        /*
        GUI.Label(new Rect(10, 10, 100, 20), "Gold: "+_playerGold);
        GUI.Label(new Rect(10, 30, 100, 20), "Key1: " + _hasKey1);
        GUI.Label(new Rect(10, 50, 100, 20), "Pot: " + _hasHealthPotion);
        GUI.Label(new Rect(10, 70, 100, 20), "CurrentMain: " + _currentMainWeaponInt);
        GUI.Label(new Rect(10, 90, 100, 20), "CurrentOff: " + _currentOffWeaponInt);
        */
    }


    public void gainKey1()
    {
        this._hasKey1 = true;
    }

    public bool getHasKey1()
    {
        return _hasKey1;
    }
    public void setHasKey1(bool b)
    {
        _hasKey1 = b;
    }
    public bool getHasPotion()
    {
        return _hasHealthPotion;
    }
    public void SetHasPotion(bool b)
    {
        _hasHealthPotion = b;
    }

    public void useHealthPotion()
    {
        this._hasHealthPotion = false;
    }

    public void gainHealthPotion()
    {
        this._hasHealthPotion = true;
    }


    public void setMw(GameObject go)
    {
        foreach(GameObject g in _mWeapons)
        {
            if(g == go)
            {
                _activeMainWeapon = g;
            }
        }

    }

    public void setOw(GameObject go)
    {
        foreach (GameObject g in _oWeapons)
        {
            if (g == go)
            {
                _activeMainWeapon = g;
            }
        }

    }


    public GameObject getMw()
    {
        return _activeMainWeapon;
    }

    public GameObject getOw()
    {
        return _activeOffWeapon;
    }
}
