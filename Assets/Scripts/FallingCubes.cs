using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCubes : MonoBehaviour {

	public float delay = 0.1f;
	private int count = 0;
	public GameObject cube;
	public Material cubeMaterial;
	private string cubeName = "FallingCube";
	string[] colorNames = { "Green", "Red", "Blue" };
	IDictionary<string, Color> colors = new Dictionary<string, Color>();

	void Start () {
		colors.Add ("Green", Color.green);
		colors.Add ("Red", Color.red);
		colors.Add ("Blue", Color.blue);
		InvokeRepeating ("Spawn", delay, delay);
	}

	void FixedUpdate(){
		this.count++;
		if(this.count > 1000){
			CancelInvoke ();
		}
	}

	void Spawn () {
		GameObject cubeClone = Instantiate (cube, new Vector3 (Random.Range (4, 10), 15, Random.Range (-8, 0)), Quaternion.identity);
		string colorName = colorNames [Random.Range (0, 3)];
		cubeClone.GetComponent<Renderer> ().material.color = colors[colorName];
		cubeClone.name = colorName + cubeName + count;
	}

	void OnApplicationQuit() {
		cube.name = cubeName;
	}
}
