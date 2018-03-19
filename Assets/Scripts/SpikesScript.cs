using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : MonoBehaviour {

    private int _spikeDamage = 4;

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerController>().Hurt(_spikeDamage);
        }

    }

}
