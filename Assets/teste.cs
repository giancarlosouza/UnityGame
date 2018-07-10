using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teste : MonoBehaviour {

	public void OnTriggerStay(Collider other){
		print (this.name + "hit " + other.gameObject.name);
	}
}
