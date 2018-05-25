using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour {

	public TcpClient mySocket;

	public string conHost = "127.0.0.1";
	public int conPort = 25000;

	public NetworkStream theStream;
	public StreamWriter theWriter;
	public StreamReader theReader;

	public bool socketReady = false;
	// Use this for initialization
	void Start () {
		try {
			mySocket = new TcpClient();
			var result = mySocket.BeginConnect(conHost,conPort,null, null);
			var success = result.AsyncWaitHandle.WaitOne(500);

			if (!success)
			{
				throw new Exception("Failed to connect.");
				socketReady = false;
			}else{

				theStream = mySocket.GetStream();
				theWriter = new StreamWriter(theStream);
				theReader = new StreamReader(theStream);
				socketReady = true;
				print("Sphere Ready");
			}

		}
		catch (Exception e) {
			Debug.Log("Socket error:" + e);
		}
	}

	/*void OnCollisionStay(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
		}
	}

	void OnTriggerStay(Collider other){
		Transform trOther = other.gameObject.GetComponent<Transform>();
		print ("On collider: " + trOther.position.z);
	}*/
}
