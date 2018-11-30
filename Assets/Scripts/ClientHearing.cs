using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System;

public class ClientHearing : MonoBehaviour {

	public TcpClient mySocket;

	public string conHost = "127.0.0.1";
	public int conPort = 2405;

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
		if(other.CompareTag ("sound")){
			audioTransformations (other);
		}
		timeNow = Time.realtimeSinceStartup;
	}

	void audioTransformations(Collider other){
		AudioSource audioSource = other.gameObject.GetComponent<AudioSource>();
		float actualVolume = audioSource.volume;

		theWriter.Write ("p(" + other.transform.name + ","
			+ other.transform.position.x + ","
			+ other.transform.position.y + ","
			+ other.transform.position.z + ","
			+ actualVolume + ").,"
			+ Time.realtimeSinceStartup.ToString () + "\n");
		theWriter.Flush ();

		float distance = Vector3.Distance (this.gameObject.transform.position, other.transform.position);
		audioSource.volume = (other.bounds.extents.y - distance) / other.bounds.extents.y;
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("surprise")) {
			AudioSource audioSource = other.gameObject.GetComponent<AudioSource>();
			audioSource.Play ();
			audioSource.volume = 1f;
			print ("Danger");
		}
	}

	public void resetTimer(){
		if(timeNow - gameTime > sendCycleTime){
			gameTime = timeNow;
		}
	}
}
