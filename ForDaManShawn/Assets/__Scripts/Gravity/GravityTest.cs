using UnityEngine;
using System.Collections;

public class GravityTest : MonoBehaviour {

	public Vector3 playerGravity;
	Rigidbody playerBody;

	public int numGravityTriggers;
	public Vector3 gravityMagnitude;
	Vector3 startGravity;

	//variables for point gravity (planets) -- should only ever have 1
	GameObject pointGravitySource;
	public bool activePG;
	float pointGravityStr;
	

	// Use this for initialization
	void Start () {
		playerBody = gameObject.GetComponent<Rigidbody>();
		playerGravity = new Vector3(0, -9.8f, 0);
		startGravity = Physics.gravity;
		//startGravity = playerGravity;
		gravityMagnitude = new Vector3(0, 0, 0);

		activePG = false;
	}
	
	// Update is called once per frame
	void Update () {
		//playerBody.velocity += playerGravity * Time.deltaTime;
	}

	//called 60 times per sec
	void FixedUpdate() {
		if (activePG) {
			//unit vector in the direciton of the planet
			Vector3 gravityDirection = (pointGravitySource.transform.position - gameObject.transform.position).normalized;
			//print(gravityDirection);
			Physics.gravity = gravityMagnitude + (gravityDirection * pointGravityStr);
			//playerGravity = gravityMagnitude + (gravityDirection * pointGravityStr);
		}

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Gravity") {
			numGravityTriggers += 1;
			gravityMagnitude += other.gameObject.GetComponent<GravityFieldData>().gravityChange; //change between += and =
			//gravityMagnitude = other.gameObject.GetComponent<GravityFieldData>().gravityChange;
			Physics.gravity = gravityMagnitude + startGravity;
			//playerGravity = gravityMagnitude + startGravity;j

		} else if (other.gameObject.tag == "PointGravity") {
			pointGravitySource = other.gameObject;
			activePG = true;
			pointGravityStr = pointGravitySource.GetComponent<GravityFieldData>().pointGravity;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Gravity") {
			numGravityTriggers -= 1;
			gravityMagnitude -= other.gameObject.GetComponent<GravityFieldData>().gravityChange; //change between -= and =
			//gravityMagnitude = new Vector3(0, 0, 0);
			Physics.gravity = gravityMagnitude + startGravity;
			//playerGravity = gravityMagnitude + startGravity;
		} else if (other.gameObject.tag == "PointGravity") {
			activePG = false;
			Physics.gravity = gravityMagnitude + startGravity;
			//playerGravity = gravityMagnitude + startGravity;
		}
	}

	//used to flip the player in positive and negative gravity
	public bool gravityPositiveY() {
		if ((gravityMagnitude.y + startGravity.y) > 0) {
			return true;
		}
		return false;
	}
}
