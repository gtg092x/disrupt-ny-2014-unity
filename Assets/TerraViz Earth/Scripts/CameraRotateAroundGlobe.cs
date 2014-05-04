//Simple camera rotate script - trimmed down version of CameraRotateAroundGlobe in TerraViz
//Created by Julien Lynge @ Fragile Earth Studios

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraRotateAroundGlobe : MonoBehaviour
{
    private float altMiles;
    public float minAltitude = 100f;
    public float maxAltitude = 15000f;

    public float lat = 30f, lon = -70f;

    public float rotateSpeed = 100f;

    void Start()
    {
        altMiles = maxAltitude / 2f;
        applyPosInfoToTransform();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Input events

    void Update()
    {
            //Move camera
		if (Input.GetMouseButton(0) || Input.GetAxis("Mouse ScrollWheel") != 0||Input.touchCount >= 1) //user is leftclick dragging - move camera along lat/lon
            {
				
			if (Input.touchCount == 1) 
			{
				if(Input.GetTouch(0).phase == TouchPhase.Moved){
					//One finger touch does orbit
					var	touch = Input.GetTouch(0);
					lon -= touch.deltaPosition.x * rotateSpeed * 0.02f;
					lat -= touch.deltaPosition.y * rotateSpeed * 0.02f;
				}

			}else if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)&&Input.touchCount ==0)
                {
                    Vector2 posChange = new Vector2(-Input.GetAxis("Mouse X") * rotateSpeed * altMiles / maxAltitude, -Input.GetAxis("Mouse Y") * rotateSpeed * altMiles / maxAltitude);

		 
				lon += posChange.x;
				lat += posChange.y;
				Debug.Log ("MOUSE AXIS");
		
			}
			
			if (Input.touchCount >= 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
			{
				Vector2 curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
				Vector2 prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
				float touchDelta = curDist.magnitude - prevDist.magnitude;

				float smoothedTime = Mathf.Sqrt(Time.deltaTime / 0.02f);
				altMiles *= 1f - Mathf.Clamp(touchDelta/15, -.8f, .4f);
				altMiles = Mathf.Clamp(altMiles, minAltitude, maxAltitude);
				
				
				
			}else if (Input.GetAxis("Mouse ScrollWheel") != 0&&Input.touchCount ==0)
                {
                    float smoothedTime = Mathf.Sqrt(Time.deltaTime / 0.02f);
                    altMiles *= 1f - Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel") * smoothedTime * 1f, -.8f, .4f);
                altMiles = Mathf.Clamp(altMiles, minAltitude, maxAltitude);
                }

                lat = Mathf.Clamp(lat, -90f, 90f);

                
            }
		applyPosInfoToTransform();
    }

    protected void applyPosInfoToTransform()
    {
        Quaternion rotation = Quaternion.Euler(lat, -lon, 0);
        Vector3 position = -(Quaternion.Euler(lat, -lon, 0) * Vector3.forward * (altMiles * 1000f / 3954.44494f + 1000f));

        transform.rotation = rotation;
        transform.position = position;
    }

    #endregion
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
