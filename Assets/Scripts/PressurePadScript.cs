using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadScript : MonoBehaviour {


    [SerializeField]
    private Sprite[] _pressurePadSprites;
    [SerializeField]
    private GameObject _dartTrap;

    private int _pressurePadState;


    // Use this for initialization
    void Start () {
        _pressurePadState = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if(_pressurePadState == 0)
        {
            GetComponent<SpriteRenderer>().sprite = _pressurePadSprites[0];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = _pressurePadSprites[1];
        }
		
	}

    public void SetPressurePadState(int i)
    {
        _pressurePadState = i;
        if(_pressurePadState == 1)
        {
            _dartTrap.GetComponent<DartTrapScript>().TriggerTrap();
        }
    }
}
