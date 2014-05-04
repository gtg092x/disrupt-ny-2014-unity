using UnityEngine;
using System.Collections;

public class plane_spawner : MonoBehaviour {

	public GameObject plane_prefab;

	private Vector2 start_coords_1 = new Vector2((float)40.7127,(float)74.0059);
	private Vector2 end_coords_1 = new Vector2((float)37.7833,(float)122.4167);

	private Vector2 start_coords_2 = new Vector2((float)30.7127,(float)60.0059);
	private Vector2 end_coords_2 = new Vector2((float)15.7833,(float)110.4167);

	// Use this for initialization
	void Start () {
	
		createPlane(start_coords_1, end_coords_1);
		createPlane(start_coords_2, end_coords_2);

	}
	
	// Update is called once per frame
	void Update () {
		


	}

	void createPlane(Vector2 start, Vector2 end){
		GameObject prefab = (GameObject)Instantiate(plane_prefab);
		plane plane = prefab.GetComponent<plane>();
		plane.init(start, end);
	}
}
