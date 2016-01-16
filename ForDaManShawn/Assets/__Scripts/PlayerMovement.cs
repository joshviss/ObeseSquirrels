using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	Rigidbody thisRigidbody;

	//constants
	float movespeed_c = 5f;         //Cap for horizontal movement speed
	float acc_c = 50f;              //How fast we get up to speed (horizontally)
	float maxJumpDuration_c = 1f;   //Maximum amount of time to apply a force to the jump
	float jumpImpulseForce_c = 6f;  //Initial impulse force applied to the jump
	float jumpAcc_c = 5.5f;			//Acceleration force applied to jumps when the button is held

	// Use this for initialization
	void Awake () {
		thisRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		//Movement controls
		if (Input.GetKey("w")) {
			Move(transform.forward);
		}
		if (Input.GetKey("s")) {
			Move(-transform.forward);
		}
		if (Input.GetKey("a")) {
			Move(-transform.right);
		}
		if (Input.GetKey("d")) {
			Move(transform.right);
		}
		//If no movement input, stop moving
		else if (!(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))) {
			thisRigidbody.velocity = new Vector3(0, thisRigidbody.velocity.y, 0);
		}

		//Jump input
		if (Input.GetKeyDown("space")) {
			Jump();
		}

		//Cap the horizontal movement speed so the player is not moving faster than movespeed_c
		CapHorizontalMovementSpeed();
	}

	void Move(Vector3 direction) {
		//Apply a force change to the rigidbody in the direction specified when called
		thisRigidbody.AddForce(direction * acc_c, ForceMode.Acceleration);
	}

	void CapHorizontalMovementSpeed() {
		//Only check and set the velocity for x and z movement
		Vector2 groundVelocity = new Vector2(thisRigidbody.velocity.x, thisRigidbody.velocity.z);
		if (groundVelocity.magnitude > movespeed_c) {
			groundVelocity = groundVelocity.normalized * movespeed_c;
			thisRigidbody.velocity = new Vector3(groundVelocity.x, thisRigidbody.velocity.y, groundVelocity.y);
		}
	}

	void Jump() {
		StartCoroutine(JumpCoroutine());
	}
	IEnumerator JumpCoroutine() {
		//Initial impulse force applied to start the jump
		thisRigidbody.AddForce(Vector3.up * jumpImpulseForce_c, ForceMode.VelocityChange);

		float timeSinceJump = 0;
		//While we haven't been jumping for too long and we are still holding the jump button
		while (timeSinceJump < maxJumpDuration_c && Input.GetKey("space")) {

			//Apply the upwards acceleration force to the rigidbody to prolong the jump
			thisRigidbody.AddForce(Vector3.up * jumpAcc_c, ForceMode.Acceleration);

			//Use fixedDeltaTime and FixedUpdate because of physics calculations
			timeSinceJump += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}
	}
}
