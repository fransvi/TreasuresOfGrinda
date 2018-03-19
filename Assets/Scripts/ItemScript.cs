using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour {



    /*
    Item types:

    _itemType:
        _itemInt:

    0: Consumable
        0: Health potion
        1: Large Coin
        2: Small Coin
        3: Key1
    1: Main Weapon
        0: Sword
        1: Mace
        2: SuperSword
    2: Off Weapon
        0: None
        1: Bow
        2: Bomb
        3: Shield
        4: MagicWand

    */
    [SerializeField]
    GameObject[] _consumable;
    [SerializeField]
    GameObject[] _mainWeapons;
    [SerializeField]
    GameObject[] _offWeapons;
    [SerializeField]
    private int _itemType;
    [SerializeField] 
    private int _itemInt; 
    private bool _isCoin;
    private Vector3 _moveDir1 = Vector3.up;
    private Vector3 _moveDir2 = Vector3.down;
    public GameObject _pickUpArrow;
    private GameObject _playerController;
    GameObject _activeItem;

    // Use this for initialization
    void Start () {
        _isCoin = false;
        _pickUpArrow.gameObject.SetActive(false);
        _playerController = GameObject.Find("Player");
        Physics2D.IgnoreLayerCollision(16, 16, true);
        Physics2D.IgnoreLayerCollision(16, 0, true);
        CheckSprite();
        CheckType();
        if(_itemType == 0 && (_itemInt == 1 || _itemInt == 2))
        {
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.size = collider.size / 3;
            _isCoin = true;
        }

	}

    public int GetItemInt()
    {
        return _itemInt;
    }
    public bool GetIsCoin()
    {
        return _isCoin;
    }


    public int GetItemType()
    {
        return _itemType;
    }
    public void SetItemType(int t)
    {
        _itemType = t;
    }
    public void Die()
    {
        Destroy(gameObject);
    }

    public void SetPickupArrowActive(bool b)
    {
        StartCoroutine(PickupArrow());
    }

    IEnumerator PickupArrow()
    {
        _pickUpArrow.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        _pickUpArrow.gameObject.SetActive(false);
    }

    public void SetItemInt(int i)
    {
        _itemInt = i;
    }
	
	// Update is called once per frame
	void Update () {

        Collider2D[] playerCheck = Physics2D.OverlapCircleAll(transform.position, 0.3f, 1<< LayerMask.NameToLayer("Default"));
        for(int i = 0; i < playerCheck.Length; i++)
        {
            if(playerCheck[i].gameObject != gameObject)
            {
                //TODO tell player lootable item nearby
            }
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.01f, LayerMask.NameToLayer("Ground") | LayerMask.NameToLayer("Stairs") 
            | LayerMask.NameToLayer("Platform"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                //Physics2D.gravity = Vector2.zero;
                //GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }

        }
        CheckType();
        CheckSprite();

       
    }

    void CheckType()
    {
        if (_itemType == 0)
        {
            _activeItem = _consumable[_itemInt];
        }
        else if (_itemType == 1)
        {
            _activeItem = _mainWeapons[_itemInt];

        }
        else if (_itemType == 2)
        {
            _activeItem = _offWeapons[_itemInt];
        }
    }

    void CheckSprite()
    {
        if (_itemType == 0)
        {
            for (int i = 0; i < _consumable.Length; i++)
            {
                if (_consumable[i] != _activeItem)
                {
                    _consumable[i].SetActive(false);
                }
                else
                {
                    _consumable[i].SetActive(true);
                }
            }
            for(int i = 0; i < _mainWeapons.Length; i++)
            {
                _mainWeapons[i].SetActive(false);
            }
            for(int i = 0; i < _offWeapons.Length; i++)
            {
                _offWeapons[i].SetActive(false);
            }
        }
        else if (_itemType == 1)
        {
            for (int i = 0; i < _mainWeapons.Length; i++)
            {

                if (_mainWeapons[i] != _activeItem)
                {
                    _mainWeapons[i].SetActive(false);
                }
                else
                {
                    _mainWeapons[i].SetActive(true);
                }
            }
            for (int i = 0; i < _consumable.Length; i++)
            {
                _consumable[i].SetActive(false);
            }
            for (int i = 0; i < _offWeapons.Length; i++)
            {
                _offWeapons[i].SetActive(false);
            }
        }
        else if (_itemType == 2)
        {
            for (int i = 0; i < _offWeapons.Length; i++)
            {
                if (_offWeapons[i] != _activeItem)
                {
                    _offWeapons[i].SetActive(false);
                }
                else
                {
                    _offWeapons[i].SetActive(true);
                }
            }
            for (int i = 0; i < _mainWeapons.Length; i++)
            {
                _mainWeapons[i].SetActive(false);
            }
            for (int i = 0; i < _consumable.Length; i++)
            {
                _consumable[i].SetActive(false);
            }
        }
    }
}

