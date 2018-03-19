using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Animator ani = GetComponent<Animator>();
        ani.Play("CoinRotation", 0, Random.Range(0.0f, 1.0f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
