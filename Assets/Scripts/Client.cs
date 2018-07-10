using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour {

	public TcpClient mySocket;

	public string conHost = "127.0.0.1";
	public int conPort = 2400;

	public NetworkStream theStream;
	public StreamWriter theWriter;

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
				socketReady = true;
				theWriter.Write(this.name + "\n");
				theWriter.Flush();
			}

		}
		catch (Exception e) {
			Debug.Log("Socket error:" + e);
		}
	}

	void OnTriggerStay(Collider other){
		Transform trOther = other.gameObject.GetComponent<Transform>();
		//print (this.name + " hit object " + trOther.name);

		theWriter.Write(this.name + " hit object " + trOther.name+"\n");
		theWriter.Flush();
	}
}
