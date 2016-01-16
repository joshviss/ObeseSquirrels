using UnityEngine;
using System.Collections;

//To be attached to the player camera
public class PlayerAim : MonoBehaviour {
	public float xSensitivity = 3f;
	public float ySensitivity = 3f;

	float yAimMin = -80f;   //yAngle clamped between these two values
	float yAimMax = 80f;    //so we aren't ever looking upside-down

	float x, y;

	float[,] originalMatrix;
	float[,] rotationMatrix;
	float[,] finalMatrix;

	Transform playerTransform;

	Quaternion desiredRotation;

	void Awake() {
		playerTransform = transform.parent;
	}

	// Update is called once per frame
	void Update() {
		//Get mouse movement input
		x += Input.GetAxis("Mouse X") * xSensitivity;
		y -= Input.GetAxis("Mouse Y") * ySensitivity;

		y = ClampRotation(y);

		//Rotate the player in the y-direction but only the camera in the x-direction
		playerTransform.rotation = Quaternion.Euler(0, x, 0);
		transform.localRotation = Quaternion.Euler(y, x, 0);
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
		desiredRotation = SetUpMatrices();
	}

	Quaternion SetUpMatrices() {
		Quaternion curRot = transform.rotation;
		// Degree of rotation
		float deg = 90;

		//Set up the originalMatrix
		originalMatrix = new float[4, 4];
		originalMatrix[0, 0] = 1 - curRot.y * curRot.y - curRot.z * curRot.z;
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
		rotationMatrix[0, 0] = Mathf.Cos(deg);
		rotationMatrix[0, 1] = Mathf.Sin(deg);
		rotationMatrix[0, 2] = 0;
		rotationMatrix[0, 3] = 0;
		rotationMatrix[1, 0] = -Mathf.Sin(deg);
		rotationMatrix[1, 1] = Mathf.Cos(deg);
		rotationMatrix[1, 2] = 0;
		rotationMatrix[1, 3] = 0;
		rotationMatrix[2, 0] = 0;
		rotationMatrix[2, 1] = 0;
		rotationMatrix[2, 2] = 1;
		rotationMatrix[2, 3] = 0;
		rotationMatrix[3, 0] = 0;
		rotationMatrix[3, 1] = 0;
		rotationMatrix[3, 2] = 0;
		rotationMatrix[3, 3] = 1;

		finalMatrix = new float[4, 4];
		finalMatrix[0, 0] = RowMultCol(0, 0);
		finalMatrix[0, 1] = RowMultCol(0, 1);
		finalMatrix[0, 2] = RowMultCol(0, 2);
		finalMatrix[0, 3] = RowMultCol(0, 3);
		finalMatrix[1, 0] = RowMultCol(1, 0);
		finalMatrix[1, 1] = RowMultCol(1, 1);
		finalMatrix[1, 2] = RowMultCol(1, 2);
		finalMatrix[1, 3] = RowMultCol(1, 3);
		finalMatrix[2, 0] = RowMultCol(2, 0);
		finalMatrix[2, 1] = RowMultCol(2, 1);
		finalMatrix[2, 2] = RowMultCol(2, 2);
		finalMatrix[2, 3] = RowMultCol(2, 3);
		finalMatrix[3, 0] = RowMultCol(3, 0);
		finalMatrix[3, 1] = RowMultCol(3, 1);
		finalMatrix[3, 2] = RowMultCol(3, 2);
		finalMatrix[3, 3] = RowMultCol(3, 3);

		Quaternion finalQuat = Quaternion.identity;
		finalQuat[3] = Mathf.Sqrt(1 + finalMatrix[0, 0] * finalMatrix[0, 1] + finalMatrix[0, 2]) / 2;
		finalQuat[0] = (finalMatrix[2, 1] - finalMatrix[1, 2]) / (finalQuat[3] * 4);
		finalQuat[1] = (finalMatrix[0, 2] - finalMatrix[2, 0]) / (finalQuat[3] * 4);
		finalQuat[2] = (finalMatrix[1, 0] - finalMatrix[0, 1]) / (finalQuat[3] * 4);
		return finalQuat;
	}

	// Multiplies the row of the rot. matrix with the col of the orig. matrix
	float RowMultCol(int row, int col) {
		return rotationMatrix[row, 0] * originalMatrix[0, col] +
			   rotationMatrix[row, 1] * originalMatrix[1, col] +
			   rotationMatrix[row, 2] * originalMatrix[2, col] +
			   rotationMatrix[row, 3] * originalMatrix[3, col];
	}
}
