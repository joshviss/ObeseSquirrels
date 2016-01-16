using UnityEngine;
using System.Collections;

//To be attached to the player camera
public class PlayerAim : MonoBehaviour {
	public float xSensitivity = 3f;
	public float ySensitivity = 3f;

	float yAimMin = -80f;	//yAngle clamped between these two values
	float yAimMax = 80f;    //so we aren't ever looking upside-down

	float x, y;

	Transform playerTransform;

	void Awake() {
		playerTransform = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		//Get mouse movement input
		x += Input.GetAxis("Mouse X") * xSensitivity;
		y -= Input.GetAxis("Mouse Y") * ySensitivity;

		y = ClampRotation(y);

		//Rotate the player in the y-direction but only the camera in the x-direction
		playerTransform.rotation = Quaternion.Euler(0, x, 0);
		transform.rotation = Quaternion.Euler(y, x, 0);
	}

	float ClampRotation(float angle) {
		//Ensure that the angle starts between -360 and 360 degrees
		if (angle < -360) {
			angle += 360;
		}
		else if (angle > 360) {
			angle -= 360;
		}

		//Clamp the angle between yAimMin and yAimMax
		return Mathf.Clamp(angle, yAimMin, yAimMax);
	}
}
