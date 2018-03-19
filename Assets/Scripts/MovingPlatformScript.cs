using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour {


    public int _moveAxis;
    private Vector3 _moveDir1;
    private Vector3 _moveDir2;
    public float _moveMultiplr;
    public bool _platformAtTop;
    public bool _platformAtBot;
    public float _moveTime;
    private bool _movingDown;
    private bool _movingUp;
    private GameObject _player;
    private bool _playerOnPlatform;

	// Use this for initialization
	void Start () {
        _playerOnPlatform = false;
        _player = GameObject.FindWithTag("Player");
        if(_moveAxis == 0)
        {
            _moveDir1 = Vector3.up;
            _moveDir2 = Vector3.down;
        }else if(_moveAxis == 1)
        {
            _moveDir1 = Vector3.left;
            _moveDir2 = Vector3.right;
        }

        _movingDown = false;
        _movingUp = false;

    }

    void Update()
    {
        Collider2D[] playerCheck = Physics2D.OverlapCircleAll(transform.position, 1f, 1 << LayerMask.NameToLayer("Default"));
        for (int i = 0; i < playerCheck.Length; i++)
        {
            if (playerCheck[i].gameObject != gameObject)
            {
                

            }

        }
        if(playerCheck.Length == 0)
        {
            _playerOnPlatform = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _playerOnPlatform = true;

        }
    }

    void OnCollisionExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            _playerOnPlatform = false;
        }
    }

    public void SetPlayerOnPlatform(bool b)
    {
        _playerOnPlatform = b;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (_platformAtTop)
        {       
            StartCoroutine(PlatTimerDown());                   
        }
        else if (_platformAtBot)
        {
            StartCoroutine(PlatTimerUp());                   
        }
        if (_movingDown)
        {
            transform.position += _moveDir2 * _moveMultiplr;
            if (_moveAxis == 1 && _playerOnPlatform)
            {
                _player.transform.position += _moveDir2 * _moveMultiplr;
            }
        }
        else if (_movingUp)
        {
            transform.position += _moveDir1 * _moveMultiplr;
            if(_moveAxis == 1 && _playerOnPlatform)
            {
                _player.transform.position += _moveDir1 * _moveMultiplr;
            }
        }
	}
    IEnumerator PlatTimerDown()
    {
        _movingDown = true;
        _platformAtTop = false;
        yield return new WaitForSeconds(_moveTime);
        _platformAtBot = true;
        _movingDown = false;
        _movingUp = true;
    }
    IEnumerator PlatTimerUp()
    {
        _movingUp = true;
        _platformAtBot = false;
        yield return new WaitForSeconds(_moveTime);
        _platformAtTop = true;
        _movingUp = false;
        _movingDown = true;
    }

}
