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
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
