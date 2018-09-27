using UnityEngine;
using System.Collections;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class Server : MonoBehaviour {


	private bool mRunning;
	public Thread mThread;
	public Thread mThread2;
	public TcpListener tcp_Listener = null;
	private TcpClient client1 = null;
	private TcpClient client2 = null;
	public string conHost = "127.0.0.1";
	public int conPort = 6322;

	public List<GameObject> sensors;

	Dictionary<string, string> perceptionSensor = 
		new Dictionary<string, string>();

	//public PlayerController player;

	byte[] bytes = new byte[1024];
	string perceptions;
	string data;

	void Start(){
		sensors = new List<GameObject>();

		tcp_Listener = new TcpListener(IPAddress.Parse(conHost), conPort);
		tcp_Listener.Start();
		print("Server Start");

		ThreadStart ts = new ThreadStart (Receive);
		mThread = new Thread (ts);
		mThread.Start ();
		ThreadStart ts2 = new ThreadStart (Receive);
		mThread2 = new Thread (ts2);
		mThread2.Start ();

		print ("Thread done...");

		mRunning = true;
	}

	void Update() {

	}

	public void addPerception(string percept, string sensor){
		perceptionSensor.Add(sensor, percept);
	}

	void Receive() {
		while (true) {
			// check if new connections are pending, if not, sleep 10ms
			if (!tcp_Listener.Pending()){
				Thread.Sleep(10);
				//print ("CCCCC");
			} else {

				TcpClient clientR = tcp_Listener.AcceptTcpClient();

				NetworkStream stream = clientR.GetStream();
				int i = 1;
				print("Server running");

				while (i != 0) {
					// Loop to receive all the data sent by the client.
					i = stream.Read(bytes, 0, bytes.Length);
					print(i);

					data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

					//byte[] msg = System.Text.Encoding.ASCII.GetBytes(perceptionSensor["1"]);
					print("Data: " + data);
					byte[] msg = System.Text.Encoding.ASCII.GetBytes ("Recebi esta merda aqui: " + data);
					stream.Write (msg, 0, msg.Length);//Send
				}
			}
		}
	}

	public void stopListening() {
		mRunning = false;
	}

	void OnApplicationQuit() { // stop listening thread
		stopListening();// wait for listening thread to terminate (max. 500ms)
		mThread.Join(500);
		mThread2.Join (500);
		tcp_Listener.Stop ();
		//UnityEditor.EditorApplication.isPlaying = false;
	}
}
