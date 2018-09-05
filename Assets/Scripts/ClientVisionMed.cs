using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System;

public class ClientVisionMed : MonoBehaviour {

	public TcpClient mySocket;

	public string conHost = "127.0.0.1";
	public int conPort = 2402;

	public NetworkStream theStream;
	public StreamWriter theWriter;

	public bool socketReady = false;

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
			}

		}
		catch (Exception e) {
			Debug.Log("Socket error:" + e);
		}
	}

	void OnTriggerStay(Collider other){
		if(!other.name.Equals("Character")){
			if (!checkOnChildrenIntersection (other)) {
				theWriter.Write(this.name + " hit object " + other.transform.name+"\n");
				theWriter.Flush();
			}
		}
	}

	Boolean checkOnChildrenIntersection(Collider other){
		if (this.gameObject.transform.childCount != 0) {
			for(int i = 0; i < this.gameObject.transform.childCount; i++){
				Bounds childBounds = this.gameObject.transform.GetChild (i).GetComponent<Collider> ().bounds;
				if(childBounds.Intersects(other.bounds)){
					return true;
				}
			}
		}
		return false;
	}
}
