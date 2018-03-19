using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControlsScript : MonoBehaviour {

    public GameObject joystick;
    public GameObject buttonMw;
    public GameObject buttonOw;
    public GameObject buttonJump;
    public GameObject buttonConsumable;

    public GameObject gameManager;
	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void JumpDown()
    {
        gameManager.GetComponent<GameManager>().MobileJumpDown();
    }

    public void JumpUp()
    {
        gameManager.GetComponent<GameManager>().MobileJumpUp();
    }
    public void MainDown()
    {
        gameManager.GetComponent<GameManager>().MobileMainDown();
    }

    public void OffDown()
    {
        gameManager.GetComponent<GameManager>().MobileOffDown();
    }
    public void OffUp()
    {
        gameManager.GetComponent<GameManager>().MobileOffUp();
    }
    public void ConsDown()
    {
        gameManager.GetComponent<GameManager>().MobileConsDown();
    }
}
