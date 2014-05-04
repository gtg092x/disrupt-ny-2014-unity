using UnityEngine;
using System.Collections;


public class plane : MonoBehaviour {

	//public Transform planet;
	public float speed = 50;
	public float height = 10; 
	public float threshhold = 30;
	private Vector3 center = Vector3.zero; 
	private float plane_radius = 1010;

	private Vector2 start_coords = new Vector2(0,0);
	private Vector2 end_coords = new Vector2(0,0);


	public void init(Vector2 start, Vector2 end){
		start_coords = start;
		end_coords = end;
		//set initial position
		transform.position = Utils.getXYZCoords(start_coords, plane_radius);
		GameObject start_sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		start_sphere.renderer.material.color = Color.yellow;
		start_sphere.transform.localScale = new Vector3(10,10,10);
		start_sphere.transform.position = Utils.getXYZCoords(start_coords, plane_radius);
		start_sphere.transform.parent = transform;

		GameObject model = transform.FindChild ("Model").gameObject;
		GameObject mesh = model.transform.FindChild ("Mesh").gameObject;
		if (Random.Range (0, 2) > 0) {
			mesh.renderer.material.color = Color.blue;
		} else {
			mesh.renderer.material.color = Color.red;
		}

	}


	// Use this for initialization
	void Start () {

	}
	

	// Update is called once per frame
	void Update () {
		//this is lat lon position
		Fly (start_coords, end_coords);

		var parent = transform.FindChild ("Sphere");
		if (parent != null) {
						GameObject sphere = parent.gameObject;
				var nextPosition = Utils.getXYZCoords (start_coords, plane_radius);
				
				sphere.transform.position = nextPosition;
		}
	}
	

	//moves plane from one point to another alonge a sphere
	void Fly(Vector2 start, Vector2 end){
		Vector3 start_point = Utils.getXYZCoords (start, plane_radius);
		Vector3 end_point = Utils.getXYZCoords (end, plane_radius);

		Vector3 dir = (end_point - start_point);
		float dist = (end_point - transform.position).magnitude;
		Vector3 axis = Vector3.Cross (start_point, end_point);

		if (dist > threshhold) {
			float dspeed = Random.Range(speed + 2, speed -2);


			var last = transform.position;
			transform.RotateAround (center, axis, Time.deltaTime * dspeed);
			var next = transform.position;

			var direction = (next-last).normalized;


			//rotate plane model to face direction of movement
			GameObject model = transform.FindChild("Model").gameObject;
			model.transform.rotation = Quaternion.LookRotation(direction);
			//model.transform.rotation = Quaternion.LookRotation(dir);


		} else {
			//if plane arrives destroy it
			Destroy(gameObject);
		}
	}

	void DrawTrail(Vector3 dir, Vector3 start, Vector3 end){

	}


	

}
