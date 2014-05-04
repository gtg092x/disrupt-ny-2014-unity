using UnityEngine;
using System.Collections;

public class clouds : MonoBehaviour {
	public float spinSpeed=2f;
	public float alpha = 1f;
	public Vector3 spinDir=Vector3.up;
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (Vector3.zero, spinDir, Time.deltaTime * spinSpeed);
		var temp=renderer.material.color;
		
		temp.a=alpha;
		renderer.material.color=temp;
	}
}
