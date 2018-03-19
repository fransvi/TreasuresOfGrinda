using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_King_Ball : MonoBehaviour {

	Collider2D[] colliders;

	public int _meleeDamage;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		colliders = Physics2D.OverlapCircleAll(transform.position, 1f, LayerMask.GetMask("Default"));
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				if (colliders[i].gameObject.GetComponent<PlayerController>())
				{
                    //KOMMENTOITU ULOS, RIKKOO KOODIN. 3.5.2017/TONI
                    colliders[i].gameObject.GetComponent<PlayerController>().Hurt(_meleeDamage);
                }

            }

		}
	}
}
