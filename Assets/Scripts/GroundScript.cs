using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.name.Contains ("Falling")) {
			Destroy (other.gameObject);
		}
	}
}
