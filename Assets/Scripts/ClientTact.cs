using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System;

public class ClientTact : MonoBehaviour {

	public TcpClient mySocket;

	public string conHost = "127.0.0.1";
	public int conPort = 2404;

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
			theWriter.Write(other.transform.name + "," 
				+ other.transform.position.x + "," 
				+ other.transform.position.y + "," 
				+ other.transform.position.z + ","
				+ Time.realtimeSinceStartup.ToString() + "\n");
			theWriter.Flush();
		}
	}
}
