using UnityEngine;
using System.Collections;

public class plane_spawner : MonoBehaviour {

	public GameObject plane_prefab;

	private Vector2 start_coords_1 = new Vector2((float)40.7127,(float)74.0059);
	private Vector2 end_coords_1 = new Vector2((float)37.7833,(float)122.4167);

	private Vector2 start_coords_2 = new Vector2((float)40.7127,(float)74.0059);
	private Vector2 end_coords_2 = new Vector2((float)37.7833,(float)122.4167);

	// Use this for initialization
	void Start () {
	
		createPlane();

	}
	
	// Update is called once per frame
	void Update () {
		




	}

	void createPlane(){
		Object plane = Instantiate(plane_prefab);
		//plane.GetComponent.<plane>().speed = 50;
	}
}
