using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour {
    [SerializeField]
    private Sprite[] _leverStateSprites;

    [SerializeField]
    private GameObject _raisingSpikes;

    [SerializeField]
    private bool _spikeLever;
    [SerializeField]
    private bool _platformLever;

    private int _leverState;
    private bool _spikesRaising;
    private bool _spikesLowering;

    private Vector3 _moveDir1;
    private Vector3 _moveDir2;
    private float _moveMultiplr;

	// Use this for initialization
	void Start () {

        _leverState = 1;
        if (_spikeLever)
        {
            _moveDir1 = Vector3.up;
            _moveDir2 = Vector3.down;
            _moveMultiplr = 0.008f;
        }else if (_platformLever)
        {
            _moveDir1 = Vector2.left;
            _moveDir2 = Vector2.right;
            _moveMultiplr = 0.04f;
        }
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(_leverState == 0)
        {
            GetComponent<SpriteRenderer>().sprite = _leverStateSprites[0];
            
        }
        else if(_leverState == 1)
        {
            GetComponent<SpriteRenderer>().sprite = _leverStateSprites[1];
            
        }

        if (_spikesRaising && !_spikesLowering)
        {
            _raisingSpikes.transform.position += _moveDir1 * _moveMultiplr;
        }
        if (_spikesLowering && !_spikesRaising)
        {
            _raisingSpikes.transform.position += _moveDir2 * _moveMultiplr;
        }
		
	}


    public void SetLeverState(int l)
    {

        if(_leverState == 0 && !_spikesLowering && !_spikesRaising)
        {
            _leverState = l;
            StartCoroutine(LowerSpikes());
        }
        else if (_leverState == 1 && !_spikesLowering && !_spikesRaising)
        {
            _leverState = l;
            StartCoroutine(RaiseSpikes());
        }
    }

    public int GetLeverState()
    {
        return _leverState;
    }

    IEnumerator RaiseSpikes()
    {
        _raisingSpikes.gameObject.SetActive(true);
        _spikesRaising = true;
        yield return new WaitForSeconds(1f);
        _spikesRaising = false;
    }

    IEnumerator LowerSpikes()
    {
        _spikesLowering = true;
        yield return new WaitForSeconds(1f);
        _raisingSpikes.gameObject.SetActive(false);
        _spikesLowering = false;
    }

    public bool GetSpikesRaising()
    {
        return _spikesRaising;
    }

    public bool GetSpikesLowering()
    {
        return _spikesRaising;
    }
}
