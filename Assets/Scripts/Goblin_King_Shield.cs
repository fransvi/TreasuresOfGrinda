using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_King_Shield : MonoBehaviour {

	Bounds playerBounds;
	Animator animator;
	GameObject player;

	public int _meleeDamage;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		//StartCoroutine (Shield ());
		player=GameObject.Find("Player(Clone)");
	}

	// Update is called once per frame
	void Update () {

		if (player.transform.position.y > transform.position.y) {
			if (!animator.GetBool ("ShieldHigh")) {
				animator.SetBool ("ShieldHigh", true);
			}
		} else {
			if (animator.GetBool ("ShieldHigh")) {
				animator.SetBool ("ShieldHigh", false);
			}
		}

		playerBounds=new Bounds(player.transform.position, new Vector3(1,1,0));

		/*
		 Debug.Log ("Intersects: "+gameObject.GetComponent<Renderer> ().bounds.Intersects (playerBounds)
			+" -- Shield: " +gameObject.GetComponent<Renderer> ().bounds+" -- Player: " +playerBounds);
		*/
		
		if (gameObject.GetComponent<Renderer> ().bounds.Intersects (playerBounds)) {
			if (player.GetComponent<PlayerController>())
			{
                player.GetComponent<PlayerController>().Hurt(_meleeDamage);
            }
        }
	}
	/*
	IEnumerator Shield(){
		while (true) { 
			if (player.transform.position.y > transform.position.y) {
				if (!animator.GetBool ("ShieldHigh")) {
					animator.SetBool ("ShieldHigh", true);
					yield return new WaitForSeconds (1);
				}
			} else {
				if (animator.GetBool ("ShieldHigh")) {
					animator.SetBool ("ShieldHigh", false);
					yield return new WaitForSeconds (1);
				}
			}
		}
	}
	*/

}
