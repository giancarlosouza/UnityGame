﻿using System.Collections;
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
		if(!other.name.Equals("Character")){
			if (this.name.Equals ("Hearing")) {
				audioTransformations (other);
			}
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

	void audioTransformations(Collider other){
		if (this.name.Equals("Hearing") && other.CompareTag ("sound")) {
			AudioSource audioSource = other.gameObject.GetComponent<AudioSource>();
			float actualVolume = audioSource.volume;
			print (actualVolume);

			float distance = Vector3.Distance (this.gameObject.transform.position, other.transform.position);
			audioSource.volume = (other.bounds.extents.y - distance) / other.bounds.extents.y;
		}
	}

	void OnTriggerEnter(Collider other){
		if (this.name.Equals("Hearing") && other.CompareTag ("surprise")) {
			AudioSource audioSource = other.gameObject.GetComponent<AudioSource>();
			audioSource.Play ();
			audioSource.volume = 1f;
			print ("Danger");
		}
	}
}
