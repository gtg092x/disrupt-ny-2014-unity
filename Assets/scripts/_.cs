﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using AssemblyCSharp;
using MiniJSON;
using System.Linq;

public class _ : MonoBehaviour {
	public bool Paused;
	public float Elapsed;

	public GameObject plane_prefab;
	public List<TripLeg> legs;
	private int legindex = 0;
	private float nextActionTime = 0.0f;

	public List<TripLeg> hotels;

	// Use this for initialization
	void Start () {
	
		LoadState(DateTime.Parse("02/13/1986"),DateTime.Parse("02/13/2036"),(state)=>{
	
			//Debug.Log (state.SelectMany(x=>x.AirLegs));
			legs = ((List<Trip>)state).SelectMany(x=>x.AirLegs).ToList();

			Debug.Log (legs);
			Debug.Log (legs.Count);
			Debug.Log (legs.Where(x=>x.StartLocation==x.EndLocation));

		});
	}


	
	// Update is called once per frame
	void Update () {

		float timeInterval = UnityEngine.Random.Range (0.5f, 1.0f);
		if (Time.time > nextActionTime ) {
			nextActionTime += timeInterval;
			if(legs!=null && legs[legindex]!=null)
			createPlane(legs[legindex].StartLocation, legs[legindex].EndLocation);
			//legindex = (legindex > legs.Count) ? 0 : legindex++;
			legindex++;
			if(legs!=null && legindex >= legs.Count){legindex=0;}


		}

	}



	void createPlane(Vector2 start, Vector2 end){
		GameObject prefab = (GameObject)Instantiate(plane_prefab);
		plane plane = prefab.GetComponent<plane>();
		plane.init(start, end);
	}
	

	List<TripLeg> getLegs(List<Trip> trips){
		var legs = trips.SelectMany (trip => trip.Bookings)
						.SelectMany (booking => booking.Segments)
						.SelectMany (seg => seg.Legs)
						.ToList ();

		Debug.Log (legs);
		return legs;
	}

	public Vector2? getLocation(IDictionary obj,string key){
		IList locationsList = obj [key] as IList;
		if (locationsList.Count == 0)
						return null;
		IDictionary featureParent = locationsList [0] as IDictionary;

		IDictionary feature = featureParent["feature"] as IDictionary;
		IDictionary geo = feature["geometry"] as IDictionary;
		float x = float.Parse (geo ["x"].ToString ());
		float y = float.Parse (geo ["y"].ToString ());
		return new Vector2 (x,y);
	}

	public void LoadState(DateTime start,DateTime end, Action<List<Trip>> onComplete){
		getJSON(String.Format("http://disrupt.digitaltaffy.com/trips-summary?startDate={0}&endDate={1}",start.ToString("MM/dd/yyyy"),end.ToString("MM/dd/yyyy")),(json)=>{

			List<Trip> result = new List<Trip>();
			IList trips = json["data"] as IList;
			foreach(IList trip in trips){
				Trip t = new Trip();
				//assign fields
				foreach(IDictionary booking in trip){
					TripBooking book = new TripBooking();
					//assign fields
					foreach(String segmentKey in booking.Keys){
						TripSegment segment = new TripSegment();
						segment.Mode = segmentKey;
						//assign fields
						IList segmentData =booking[segmentKey] as IList;
						foreach(IDictionary leg in segmentData){
							TripLeg tripLeg = new TripLeg();
							//assign fields
							tripLeg.Start = DateTime.Parse(leg["StartDateUtc"] as string);
							tripLeg.End = DateTime.Parse(leg["EndDateUtc"] as string);
							tripLeg.Name = leg["Name"] as String;

							Vector2? startLocation = getLocation(leg,"StartLocations");
							if(startLocation==null)
								continue;
							tripLeg.StartLocation = startLocation.Value;

							Vector2? endLocation = getLocation(leg,"EndLocations");

							if(endLocation.HasValue)
								tripLeg.EndLocation = endLocation.Value;
							else
								tripLeg.EndLocation=tripLeg.StartLocation;


							segment.Legs.Add(tripLeg);
						}
						book.Segments.Add(segment);
					}

					t.Bookings.Add(book);
				}
				result.Add (t);
			}
			onComplete(result);
		});
	}


	void FixedUpdate(){
		if (!Paused)
				Elapsed += Time.fixedDeltaTime;
	}
	void Reset(){
		Elapsed = 0f;
	}



	public void getJSON(string data,Action<IDictionary> onComplete){
		getWeb(data,(result)=>onComplete(parseJson(result)));
	}

	public IDictionary parseJson(string data){
		//MiniJSON parser = new MiniJSON ();
		return MiniJSON.Json.Deserialize (data) as IDictionary;
	}

	public void getWeb(string url,Action<String> onComplete){

		Consumer c = new Consumer (url, onComplete);
		StartCoroutine (c.Start());
	}


}
