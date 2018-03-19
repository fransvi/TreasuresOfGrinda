using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour {

    [SerializeField]
    private int _meleeDamage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame

     
	void FixedUpdate () {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f, LayerMask.GetMask("Default"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if (colliders[i].gameObject.GetComponent<PlayerController>())
                {
                    //KOMMENTOITU ULOS, RIKKOO KOODIN. 3.5.2017/TONI
                    //colliders[i].gameObject.GetComponent<PlayerController>().Hurt(_meleeDamage);
                }

            }

        }

    }
}
