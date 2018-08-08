using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text; 
using System;

public class PlayerController : MonoBehaviour {

	public float speed;
	private Rigidbody rb;
	private Transform tr;
	private string movementNow = "0, 0\n";

	public string conHost = "127.0.0.1";
	public int conPort = 1220;

	public TcpListener server;
	public TcpClient mySocket;
	public Thread mThread;

	public NetworkStream theStream;
	public StreamWriter theWriter;
	public StreamReader theReader;

	public bool socketReady = false;
	private bool mRunning;

	public List<string> zPos;
	private string otherZPos = "\n";

	void Start(){
		speed = 10;
		rb = GetComponent<Rigidbody> ();
		tr = GetComponent<Transform> ();
		zPos = new List<string> ();

		/*mThread = new Thread (new ThreadStart(Listen)); 		
		mThread.IsBackground = true; 		
		mThread.Start();*/
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
		/*if (theStream != null && theStream.DataAvailable) {
			Byte[] inStream = new Byte[mySocket.SendBufferSize];
			theStream.Read(inStream, 0, inStream.Length);
			print (System.Text.Encoding.UTF8.GetString (inStream).TrimEnd ('\0'));
			//movementNow = doSomething(System.Text.Encoding.UTF8.GetString(inStream).TrimEnd('\0'));
		}*/
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

	public void Listen(){
		try { 			
			// Create listener on localhost port 8052. 			
			server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8052); 			
			server.Start();              
			Debug.Log("Server is listening");              
			Byte[] bytes = new Byte[1024];  			
			while (true) { 				
				using (mySocket = server.AcceptTcpClient()) { 					
					// Get a stream object for reading 					
					using (NetworkStream stream = mySocket.GetStream()) { 						
						int length; 						
						// Read incomming stream into byte arrary. 						
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 							
							var incommingData = new byte[length]; 							
							Array.Copy(bytes, 0, incommingData, 0, length);  							
							// Convert byte array to string message. 							
							string clientMessage = Encoding.ASCII.GetString(incommingData); 							
							Debug.Log("client message received as: " + clientMessage); 						
						} 					
					} 				
				} 			
			} 		
		} 		
		catch (SocketException socketException) { 			
			Debug.Log("SocketException " + socketException.ToString()); 		
		}
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