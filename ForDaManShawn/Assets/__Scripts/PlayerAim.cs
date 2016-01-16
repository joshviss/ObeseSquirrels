using UnityEngine;
using System.Collections;

//To be attached to the player camera
public class PlayerAim : MonoBehaviour {
	public float xSensitivity = 3f;
	public float ySensitivity = 3f;

	float yAimMin = -80f;	//yAngle clamped between these two values
	float yAimMax = 80f;    //so we aren't ever looking upside-down

	float x, y;

	float[,] originalMatrix;
	float[,] rotationMatrix;

	Transform playerTransform;

	Quaternion desiredRotation;

	//TEST TODO
	bool flipping, pPositiveY;
	GravityTest playerGravity;
	public float flipTime = 0.25f;

	void Awake() {
		playerTransform = transform.parent;
		playerGravity = playerTransform.GetComponent<GravityTest>();

		//Hide the mouse and lock it to the screen
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;

		//the player is not currently flipping
		flipping = false;
		pPositiveY = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!flipping) {
			//Get mouse movement input
			x += Input.GetAxis("Mouse X") * xSensitivity;
			y -= Input.GetAxis("Mouse Y") * ySensitivity;

			y = ClampRotation(y);

			//Rotate the player in the y-direction but only the camera in the x-direction
			bool gravityPositiveY = playerGravity.gravityPositiveY();

			//gets the correct rotation on player
			if (!pPositiveY && gravityPositiveY) { //take out if not finished
				//start coroutine to flip to pos Y
				flipping = true;
				pPositiveY = true;
				StartCoroutine("FlipPosY");
			} else if(gravityPositiveY) { //when flipped
				playerTransform.rotation = Quaternion.Euler(0 + 180, -x, 0);
				transform.rotation = Quaternion.Euler(y + 180, -x, 0);
			} else if (pPositiveY){ //take out if not finished
				//start coroutine to flip to normal neg Y
				flipping = true;
				pPositiveY = false;
				StartCoroutine("FlipNegY");
			} else {
				playerTransform.rotation = Quaternion.Euler(0, x, 0);
				transform.rotation = Quaternion.Euler(y, x, 0);
			}

			/*
			//changes the orientation of the player TEST TODO
			if (testNum == 0) {
				print("hello");
				Vector3 targetUp = new Vector3(0, -1, 0);
				float damping = 8;
				gameObject.transform.up = Vector3.Slerp(transform.up, targetUp, Time.deltaTime * damping);
				testNum += 1;
			}
			 */
		}
	}

	IEnumerator FlipPosY() {
		float timeIn;
		float flipStartTime = Time.time;
		float percentComp = 0;

		while (percentComp < 1) {
			//calculates the percent complete
			timeIn = Time.time - flipStartTime;
			percentComp = timeIn / flipTime;

			if (percentComp > 1) {
				percentComp = 1;
			}

			//Get mouse movement input
			x += Input.GetAxis("Mouse X") * xSensitivity;
			y -= Input.GetAxis("Mouse Y") * ySensitivity;

			y = ClampRotation(y);

			//Quaternion.Lerp(startpoint, endpoint, percentageBetweenTwoVal) //faster but used Slerp
			playerTransform.rotation = Quaternion.Slerp(Quaternion.Euler(0, x, 0), Quaternion.Euler(0 + 180, -x, 0), percentComp);
			transform.rotation = Quaternion.Slerp(Quaternion.Euler(y, x, 0), Quaternion.Euler(y + 180, -x, 0), percentComp);

			yield return new WaitForFixedUpdate();
		}

		flipping = false;
	}

	IEnumerator FlipNegY() {
		float timeIn;
		float flipStartTime = Time.time;
		float percentComp = 0;

		while (percentComp < 1) {
			//calculates the percent complete
			timeIn = Time.time - flipStartTime;
			percentComp = timeIn / flipTime;

			if (percentComp > 1) {
				percentComp = 1;
			}

			//Get mouse movement input
			x += Input.GetAxis("Mouse X") * xSensitivity;
			y -= Input.GetAxis("Mouse Y") * ySensitivity;

			y = ClampRotation(y);

			//Quaternion.Lerp(startpoint, endpoint, percentageBetweenTwoVal) //faster but used Slerp
			playerTransform.rotation = Quaternion.Slerp(Quaternion.Euler(0 + 180, -x, 0), Quaternion.Euler(0, x, 0), percentComp);
			transform.rotation = Quaternion.Slerp(Quaternion.Euler(y + 180, -x, 0), Quaternion.Euler(y, x, 0), percentComp);

			yield return new WaitForFixedUpdate();
		}

		flipping = false;
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

	public void Rotate(Vector3 targetRotation) {
		print(desiredRotation);
		desiredRotation = Quaternion.Euler(targetRotation);
		print(desiredRotation);
	}

	void SetUpMatrices() {
		Quaternion curRot = transform.rotation;
		//Set up the originalMatrix
		originalMatrix = new float[4, 4];
		originalMatrix[0, 0] = 1 - curRot.y * curRot.y - curRot.z*curRot.z;
		originalMatrix[0, 1] = curRot.x * curRot.y - curRot.w * curRot.z;
		originalMatrix[0, 2] = curRot.x * curRot.z + curRot.y * curRot.w;
		originalMatrix[0, 3] = 0;
		originalMatrix[1, 0] = curRot.x * curRot.y + curRot.w * curRot.z;
		originalMatrix[1, 1] = 1 - curRot.x * curRot.x - curRot.z * curRot.z;
		originalMatrix[1, 2] = curRot.y * curRot.z - curRot.w * curRot.x;
		originalMatrix[1, 3] = 0;
		originalMatrix[2, 0] = curRot.w * curRot.z - curRot.w * curRot.y;
		originalMatrix[2, 1] = curRot.y * curRot.z + curRot.w * curRot.x;
		originalMatrix[2, 2] = 1 - curRot.x * curRot.x - curRot.y * curRot.y;
		originalMatrix[2, 3] = 0;
		originalMatrix[3, 0] = 0;
		originalMatrix[3, 1] = 0;
		originalMatrix[3, 2] = 0;
		originalMatrix[3, 3] = 1;

		rotationMatrix = new float[4, 4];
	}
}
