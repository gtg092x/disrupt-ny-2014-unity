using UnityEngine;
using System.Collections;

public class clouds : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (Vector3.zero, Vector3.up, Time.deltaTime * 2);
	}
}
