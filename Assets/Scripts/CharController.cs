using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text; 
using System;

public class CharController : MonoBehaviour {

	public float speed;
	public float turnSpeed;

	public string conHost = "127.0.0.1";
	public int conPort = 2600;
	public TcpListener server;
	public TcpClient mySocket;

	public Thread mThread;
	private Thread threadTimer;
	private bool running = true;

	public ClientVisionFar visionFar;
	public ClientVisionMed visionMed;
	public ClientVisionClose visionClose;
	public ClientTact tact;
	public ClientHearing hearing;

	void Start(){
		speed = 10;
		turnSpeed = 2;
		mThread = new Thread (new ThreadStart(Listen)); 		
		mThread.IsBackground = true; 		
		mThread.Start();
		threadTimer = new Thread ((new ThreadStart(testando)));
		threadTimer.Start ();
	}

	public void FixedUpdate () {

		CharacterController controller = GetComponent<CharacterController> ();
		Animation an = GetComponent<Animation> ();

		Vector3 move = new Vector3 (0, 0, Input.GetAxis ("Vertical"));

		if (Input.GetAxis ("Vertical") != 0) {
			an.Play ("run");
		} else {
			an.Play ("idle");
		}
			
		controller.transform.Rotate (0, Input.GetAxis ("Horizontal") * turnSpeed, 0);
		move = transform.TransformDirection (move);
		controller.SimpleMove (move * speed);

	}

	public void Listen(){
		try { 			
			// Create listener on localhost port 2600. 			
			server = new TcpListener(IPAddress.Parse("127.0.0.1"), conPort); 			
			server.Start();              
			Debug.Log("Server is listening");              
			Byte[] bytes = new Byte[1024];  			
			while (running) { 				
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

	public void testando(){
		while(running){
			visionFar.resetTimer ();
			visionMed.resetTimer ();
			visionClose.resetTimer ();
			tact.resetTimer ();
			hearing.resetTimer ();
		}
	}

	void OnApplicationQuit() { // stop listening thread
		running = false;
		threadTimer.Join(500);
		mThread.Join(500);
	}
}