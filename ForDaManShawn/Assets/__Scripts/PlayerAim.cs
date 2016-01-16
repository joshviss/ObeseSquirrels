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

	void Awake() {
		playerTransform = transform.parent;

		//Hide the mouse and lock it to the screen
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
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

        //Set the cursor back to visible if you hit escape
        if (Input.GetKeyUp(KeyCode.Escape))
            Cursor.visible = !Cursor.visible;
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
