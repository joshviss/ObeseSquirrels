using UnityEngine;
using System.Collections;

public class GravityChange : MonoBehaviour {
	float gravityMagnitude;

	// Use this for initialization
	void Start () {
		gravityMagnitude = Physics.gravity.magnitude;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("right")) {
			SwitchGravity(Vector3.right);
		}
	}

	void SwitchGravity(Vector3 gravityDirection) {
		Physics.gravity = gravityDirection * gravityMagnitude;

		//Rotate the player so that gravity direction is down
		//GameManager.S.playerMovement.Rotate(-gravityDirection);
	}
}
