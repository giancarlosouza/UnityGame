using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System;

public class PlayerController : MonoBehaviour {

	public float speed;
	private Rigidbody rb;
	private Transform tr;
	private string movementNow = "0, 0\n";

	public string conHost = "127.0.0.1";
	public int conPort = 2400;

	public TcpClient mySocket;

	public NetworkStream theStream;
	public StreamWriter theWriter;
	public StreamReader theReader;

	public bool socketReady = false;

	public List<string> zPos;
	private string otherZPos = "\n";

	void Start(){
		rb = GetComponent<Rigidbody> ();
		tr = GetComponent<Transform> ();
		zPos = new List<string> ();

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
				theWriter.Write("Agent\n");
				theWriter.Flush();
			}

		}
		catch (Exception e) {
			Debug.Log("Socket error:" + e);
		}
	}

	public void FixedUpdate () {

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);

		/*String tmpString = moveHorizontal + ", " + moveVertical + "\n";
		theWriter.Write(tmpString);
		theWriter.Flush();*/

		//String result = "";
		if (theStream != null && theStream.DataAvailable) {
			Byte[] inStream = new Byte[mySocket.SendBufferSize];
			theStream.Read(inStream, 0, inStream.Length);
			print (System.Text.Encoding.UTF8.GetString (inStream).TrimEnd ('\0'));
			//movementNow = doSomething(System.Text.Encoding.UTF8.GetString(inStream).TrimEnd('\0'));
		}
	}

	public string doSomething(string move){
		print (move.Length);
		Vector3 movement;
		if (move.Equals ("W")) {
			print ("cima");
			movement = new Vector3 (0.0f, 0.0f, 1.0f);
		} else {
			print ("baixo");
			movement = new Vector3 (0.0f, 0.0f, -1.0f);
		}
		this.rb.AddForce (movement * speed);
		return movement.x + ", " + movement.z + "\n";
	}

	/*public void getCloser(string move, float otherZPosition){
		while (Math.Abs (otherZPosition - this.tr.position.z) > 1) {
			print ("Entrou");
			Vector3 movement;
			if (move.Equals ("W")) {
				print ("cima");
				movement = new Vector3 (0.0f, 0.0f, 1.0f);
			} else {
				print ("baixo");
				movement = new Vector3 (0.0f, 0.0f, -1.0f);
			}
			this.rb.AddForce (movement * speed);
		}
	}*/

	/*void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Vanish")){
			//other.gameObject.SetActive (false);
			Transform trOther = other.gameObject.GetComponent<Transform>();

			zPos.Add(this.tr.position.z + "," + trOther.position.z);
		}
	}*/

	/*public void OnTriggerStay(Collider other){
		Transform trOther = other.gameObject.GetComponent<Transform>();
		//print ("On collider: " + trOther.position.z);

		theWriter.Write(trOther.position.z.ToString()+"\n");
		theWriter.Flush();

		String result = "";
		if (theStream != null && theStream.DataAvailable) {
			Byte[] inStream = new Byte[mySocket.SendBufferSize];
			theStream.Read(inStream, 0, inStream.Length);
			print (System.Text.Encoding.UTF8.GetString (inStream).TrimEnd ('\0'));
		}
	}*/
}