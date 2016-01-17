using UnityEngine;
using System.Collections;

public class Lobby : MonoBehaviour {
	public static Lobby S;
	public int numKeysInLock {
		get {
			return _numKeysInLock;
		}
		set {
			_numKeysInLock = value;
			//If all three keys are in locks, do something
			if (value == 3) {
				UnlockFinalDoor();
			}
		}
	}
	int _numKeysInLock;

	GameObject finalDoorFrame;
	float doorOpenTime = 3f;
	float doorOpenDistance = 1.1f;

	// Use this for initialization
	void Start () {
		S = this;
		_numKeysInLock = 0;
		finalDoorFrame = GameObject.Find("FinalDoorFrame");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag != "Player") {
			return;
		}

		PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
		if (player.keyOnPlayer == null) {
			return;
		}
		player.keyOnPlayer.state = Key.KeyState.inLobby;
		player.keyOnPlayer = null;
	}

	void UnlockFinalDoor() {
		GameObject leftDoor = finalDoorFrame.transform.FindChild("ClosedDoorLeft").gameObject;
		GameObject rightDoor = finalDoorFrame.transform.FindChild("ClosedDoorRight").gameObject;

		StartCoroutine(OpenFinalDoorCoroutine(leftDoor, rightDoor));
	}

	IEnumerator OpenFinalDoorCoroutine(GameObject left, GameObject right) {
		Vector3 startLeft = left.transform.position;
		Vector3 startRight = right.transform.position;

		Vector3 targetLeft = startLeft + left.transform.right * doorOpenDistance;
		Vector3 targetRight = startRight - right.transform.right * doorOpenDistance;

		float t = 0;
		while (t < doorOpenTime) {
			float percent = t / doorOpenTime;
            left.transform.position = Vector3.Lerp(startLeft, targetLeft, percent * percent);
			right.transform.position = Vector3.Lerp(startRight, targetRight, percent * percent);

			t += Time.deltaTime;
			yield return 0;
		}
	}
}
