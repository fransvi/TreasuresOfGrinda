using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {


    [SerializeField]
    Sprite[] _doorStates;
    private Animator _animator;


    public bool _isBossDoor;
    private bool _bossDoorOpening; private bool _bossDoorOpen;
    public bool _requiresKey;

    private int _doorState;
	// Use this for initialization
	void Start () {
        _doorState = 0;
        _bossDoorOpening = false; _bossDoorOpen = false;
        if (_isBossDoor)
        {
            _animator = GetComponent<Animator>();
        }
	}
	
	// Update is called once per frame
	void Update () {



        if (_isBossDoor)
        {

            if (!_bossDoorOpening && !_bossDoorOpen)
            {
                _animator.Play("BossDoorIdle");
            }
            else if (_bossDoorOpen)
            {
                _animator.Play("BossDoorIdleOpen");
            }

            if (_doorState == 0)
            {

            }
            else if(_doorState == 1 && !_bossDoorOpen)
            {
                StartCoroutine(OpenBossDoor());
            }
           

        }
        else
        {
            if (_doorState == 0)
            {
                GetComponent<SpriteRenderer>().sprite = _doorStates[0];
            }
            else if (_doorState == 1)
            {

                GetComponent<SpriteRenderer>().sprite = _doorStates[1];
            }
        }

	}
    IEnumerator OpenBossDoor()
    {
        _bossDoorOpening = true;
        _animator.Play("BossDoorOpen");
        yield return new WaitForSeconds(2f);
        _bossDoorOpen = true;
        _bossDoorOpening = false;
    }
    public int GetDoorState()
    {
        return _doorState;
    }
    public bool GetRequiresKey()
    {
        return _requiresKey;
    }

    public void SetDoorState(int ds)
    {
        _doorState = ds;
    }
}
