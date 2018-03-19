using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLocked : MonoBehaviour {

	bool hasKey=true; // Placeholderi; TODO Hae boolean pelaajan inventorysta
	bool doorOpened=false;
	public Renderer player;

	bool playerDoorIntersect;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (doorOpened==false){
			playerDoorIntersect = gameObject.GetComponent<Renderer> ().bounds.Intersects (player.bounds);
			if (hasKey==true && Input.GetKeyUp(KeyCode.LeftControl) == true && playerDoorIntersect){
				doorOpened = true;
				Debug.Log ("doorOpened");
				// TODO: Avaa ovi
			}
		}
	}
}
