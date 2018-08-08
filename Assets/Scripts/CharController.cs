using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text; 
using System;

public class CharController : MonoBehaviour {

	public float speed;
	public float turnSpeed;

	void Start(){
		speed = 10;
		turnSpeed = 2;
	}

	public void FixedUpdate () {

		CharacterController controller = GetComponent<CharacterController> ();
		Animation an = GetComponent<Animation> ();

		Vector3 move = new Vector3 (0, 0, Input.GetAxis ("Vertical"));

		if (Input.GetAxis ("Vertical") != 0) {
			an.Play ("run");
		} else {
			an.Play ("idle");
		}
			
		controller.transform.Rotate (0, Input.GetAxis ("Horizontal") * turnSpeed, 0);
		move = transform.TransformDirection (move);
		controller.SimpleMove (move * speed);

	}
}