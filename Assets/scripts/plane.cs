﻿using UnityEngine;
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
		transform.position = getXYZCoords(start_coords);
	}


	// Use this for initialization
	void Start () {

	}
	

	// Update is called once per frame
	void Update () {
		//this is lat lon position
		Fly (start_coords, end_coords);
	}
	

	//moves plane from one point to another alonge a sphere
	void Fly(Vector2 start, Vector2 end){
		Vector3 start_point = getXYZCoords(start);
		Vector3 end_point = getXYZCoords(end);

		Vector3 dir = (end_point - start_point);
		float dist = (end_point - transform.position).magnitude;
		Vector3 axis = Vector3.Cross (start_point, end_point);

		if (dist > threshhold) {
			transform.RotateAround (center, axis, Time.deltaTime * speed);
		} else {
			//if plane arrives destroy it
			Destroy(gameObject);
		}
	}


	//gets xyz coords from lat lon position and a given radius
	Vector3 getXYZCoords(Vector2 coords){
		Vector3 pos;
		//multiply my -1 because of the way the earth model is set up
		float lat = coords.x * -1;
		float lon = coords.y;
		
		float cosLat = Mathf.Cos(lat * Mathf.PI / 180);
		float sinLat = Mathf.Sin(lat * Mathf.PI / 180);
		float cosLon = Mathf.Cos(lon * Mathf.PI / 180);
		float sinLon = Mathf.Sin(lon * Mathf.PI / 180);

		pos.x = plane_radius * cosLat * cosLon;
		pos.y = plane_radius * cosLat * sinLon;
		pos.z = plane_radius * sinLat;
		return pos;
	}



}
