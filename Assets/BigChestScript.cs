using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigChestScript : MonoBehaviour {


    [SerializeField]
    Sprite[] _chestStates;
    [SerializeField]
    private Transform _openPoint;
    [SerializeField]
    private GameObject[] _gameObjects;

    [SerializeField]
    private int _chestContainsInt;
    [SerializeField]
    private int _chestContainsType;

    [SerializeField]
    private GameObject _chestContainsItem;

    [SerializeField]
    private int _chestContainsAmount;

    private int _chestState;
    private bool _isBigChest = true;

    private bool _chestFalling; private bool _chestOpen;
    private Animator _animator;

    

    void Start()
    {
        _chestFalling = false; _chestOpen = false;
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (!_chestFalling && !_chestOpen)
        {
            _animator.Play("BigChestIdle");
        }
        else if(_chestFalling && !_chestOpen)
        {
            _animator.Play("BigChestFall");
        }else if (_chestOpen)
        {
            _animator.Play("BigChestOpen");
        }

    }

    public void PlayFallAnimation()
    {
        StartCoroutine(ChestFalling());
    }

    IEnumerator ChestFalling()
    {
        _chestFalling = true;
        yield return new WaitForSeconds(3f);
        _chestFalling = false;
    }
    IEnumerator BigChestGoldSpray()
    {

        yield return new WaitForSeconds(7.5f);
        CancelInvoke("BigChestGoldSprayRepeat");
        yield return new WaitForSeconds(10f);
        Application.LoadLevel("EndShow");
    }


    private void BigChestGoldSprayRepeat()
    {
        Vector3 newPos = new Vector3(_openPoint.position.x, _openPoint.position.y, _openPoint.position.z - 2);
        GameObject go = Instantiate(_chestContainsItem, newPos, transform.rotation);
        go.GetComponent<ItemScript>().SetItemInt(_chestContainsInt);
        go.GetComponent<ItemScript>().SetItemType(_chestContainsType);
        float itemForcey = Random.Range(150, 300);
        float itemForcex = Random.Range(-60, 60);
        go.GetComponent<Rigidbody2D>().AddForce(new Vector2(itemForcex, itemForcey));
    }
    public void OpenChest()
    {
        gameObject.SetActive(true);
        _chestOpen = true;
        if (_isBigChest)
        {
            _chestState = 0;
        }
        if (_chestState == 0)
        {
            for (int i = 0; i < _chestContainsAmount; i++)
            {
                Vector3 newPos = new Vector3(_openPoint.position.x, _openPoint.position.y, _openPoint.position.z - 2);
                /*
                Vector3 euler = transform.eulerAngles;
                euler.z = Random.Range(0f, 360f);
                _openPoint.eulerAngles = euler;
                */

               
            }
            InvokeRepeating("BigChestGoldSprayRepeat", 0f, 0.05f);
            StartCoroutine(BigChestGoldSpray());
            _chestState = 1;
        }

    }
}

