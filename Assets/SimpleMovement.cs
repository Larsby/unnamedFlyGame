using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = transform.position + transform.forward * Time.deltaTime * 0.3f;
	//	GetComponent<Rigidbody>().velocity = transform.forward * Time.deltaTime * 6.0f;
	}
}
