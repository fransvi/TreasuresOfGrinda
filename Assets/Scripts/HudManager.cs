using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour {

    [SerializeField]
    private Sprite[] _healthBar;
    [SerializeField]
    private Image _currentHealthSprite;
    [SerializeField]
    private Sprite[] _consumableSprites;
    [SerializeField]
    private Sprite[] _mainWeaponSprites;
    [SerializeField]
    private Sprite[] _offWeaponSprites;
    [SerializeField]
    private Sprite[] _slotSprites;
    [SerializeField]
    private Image[] _current2Slot;
    [SerializeField]
    private Image[] _current3Slot;
    [SerializeField]
    private Image[] _current4Slot;
    [SerializeField]
    private GameObject _current1Button;
    [SerializeField]
    private GameObject _current2Button;
    [SerializeField]
    private GameObject _current3Button;
    [SerializeField]
    private Image _keySlot;
    [SerializeField]
    private Text _coinsText;
    [SerializeField]
    private Sprite[] _QWEsprites;
    [SerializeField]
    private Sprite[] _ZXCsprites;

    public GameObject[] _mobileUI;
    public GameObject[] _pcUI;

    public bool _MobileControls;

    public int _controlsState;

    [SerializeField]
    private GameObject _playerController;
    private GameObject _playerManager;


    public void SetPlayerStats(GameObject player, GameObject inventory)
    {
        _playerController = player;
        _playerManager = inventory;
        
    }

    public void SetMobileControls(bool t)
    {
        _MobileControls = t;
    }

    public void SetControlState(int cs)
    {
        _controlsState = cs;

        if (_controlsState == 0)
        {
            _current1Button.GetComponent<Image>().sprite = _QWEsprites[0];
            _current2Button.GetComponent<Image>().sprite = _QWEsprites[1];
            _current3Button.GetComponent<Image>().sprite = _QWEsprites[2];
        }
        else if (_controlsState == 1)
        {
            _current1Button.GetComponent<Image>().sprite = _ZXCsprites[0];
            _current2Button.GetComponent<Image>().sprite = _ZXCsprites[1];
            _current3Button.GetComponent<Image>().sprite = _ZXCsprites[2];
        }
    }
    public int GetControlsState()
    {
        return _controlsState;
    }

	// Update is called once per frame
	void Update () {

        int hp = _playerController.GetComponent<PlayerController>().GetHealth();
        _currentHealthSprite.overrideSprite = _healthBar[hp];

        

        bool hasPotion = _playerManager.GetComponent<PlayerInventory>().getHasPotion();
        bool hasKey = _playerManager.GetComponent<PlayerInventory>().getHasKey1();
        float goldAmount = _playerManager.GetComponent<PlayerInventory>().GetCurrentGold();
        int mainWeapon = _playerManager.GetComponent<PlayerInventory>().GetCurrentMainWeapon();
        int offWeapon = _playerManager.GetComponent<PlayerInventory>().GetCurrentOffWeapon();


        _coinsText.text = "x" + goldAmount;

        if (_MobileControls)
            {
                _current3Slot[1].gameObject.SetActive(true);
                if(offWeapon > 0)
                {
                    _current3Slot[1].sprite = _offWeaponSprites[offWeapon];
                }
                else
                {
                    _current3Slot[1].gameObject.SetActive(false);
                }
                _current2Slot[1].gameObject.SetActive(true);
                _current2Slot[1].sprite = _mainWeaponSprites[mainWeapon];
                _current3Slot[0].gameObject.SetActive(false);
            _current2Slot[0].gameObject.SetActive(false);
            }
            else
            {
                _current3Slot[0].gameObject.SetActive(true);
                if (offWeapon > 0)
                {
                _current3Slot[0].sprite = _offWeaponSprites[offWeapon];
                }
                else
                {
                _current3Slot[0].gameObject.SetActive(false);
                }
                _current2Slot[0].gameObject.SetActive(true);
                _current2Slot[0].sprite = _mainWeaponSprites[mainWeapon];
                _current3Slot[1].gameObject.SetActive(false);
            _current2Slot[1].gameObject.SetActive(false);
        }



        



        if (hasPotion)
        {
            if (_MobileControls)
            {
                _current4Slot[1].gameObject.SetActive(true);
                _current4Slot[1].sprite = _consumableSprites[0];
                _current4Slot[0].gameObject.SetActive(false);
            }
            else
            {
                _current4Slot[0].gameObject.SetActive(true);
                _current4Slot[0].sprite = _consumableSprites[0];
                _current4Slot[1].gameObject.SetActive(false);
            }

        }
        else
        {
            _current4Slot[0].gameObject.SetActive(false);
            _current4Slot[1].gameObject.SetActive(false);
        }
        if(hasKey)
        {
            _keySlot.gameObject.SetActive(true);
        }
        else
        {
            _keySlot.gameObject.SetActive(false);
        }
		
	}
}
