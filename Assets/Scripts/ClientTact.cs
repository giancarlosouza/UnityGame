﻿using System.Collections;
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

	private float gameTime;
	private float timeNow;
	public float timeToWaitBeforeSendPerception = 0.05f;
	public float timeRangeToSendPerception = 0.0005f;
	private float sendCycleTime;

	void Start () {
		gameTime = Time.realtimeSinceStartup;
		sendCycleTime = timeToWaitBeforeSendPerception + timeRangeToSendPerception;
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
			theWriter.Write ("p(" + other.transform.name + ","
				+ other.transform.position.x + ","
				+ other.transform.position.y + ","
				+ other.transform.position.z + ").,"
				+ Time.realtimeSinceStartup.ToString () + "\n");
			theWriter.Flush ();
		}
		timeNow = Time.realtimeSinceStartup;
	}

	public void resetTimer(){
		if(timeNow - gameTime > sendCycleTime){
			gameTime = timeNow;
		}
	}
}
