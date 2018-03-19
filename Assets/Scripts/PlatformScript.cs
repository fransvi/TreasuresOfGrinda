using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Player")
        {
            Debug.Log("Colliding....");
            Physics2D.IgnoreCollision(other.GetComponent<CircleCollider2D>(), transform.GetComponent<BoxCollider2D>());
        }

    }

    void onTriggerExit(Collider other)
    {

        if (other.gameObject.name == "Player")
        {
            other.gameObject.layer = 0;
            Debug.Log("Exiting collision....");
            Physics2D.IgnoreCollision(other.GetComponent<CircleCollider2D>(), transform.GetComponent<BoxCollider2D>(), false);
        }

    }
}
