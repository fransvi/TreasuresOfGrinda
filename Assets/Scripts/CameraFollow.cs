using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    Transform target;
    public float leftLimit;
    public float rightLimit;
    public float topLimit;
    public float bottomLimit;
    Vector3 originalCameraPosition;

    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        /*
        Vector3 NewCamera = target.position;
        NewCamera.z = transform.position.z;
        transform.position = NewCamera;
        */
        Vector3 fixedPos = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.y);
        fixedPos.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, fixedPos, Time.deltaTime * Mathf.Clamp((fixedPos - transform.position).sqrMagnitude * 8, .1f, 5));
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), Mathf.Clamp(transform.position.y, bottomLimit, topLimit), transform.position.z);
        originalCameraPosition = transform.position;
    }
}
