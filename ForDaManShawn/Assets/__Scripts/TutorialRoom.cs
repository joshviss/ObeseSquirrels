using UnityEngine;
using System.Collections;

public class TutorialRoom : MonoBehaviour {
	public static TutorialRoom S;
	public GameObject leftDoor, rightDoor, lobbyPortal;
	float doorOpenTime = 3f;
	float doorOpenDistance = 1.1f;

	// Use this for initialization
	void Start () {
		S = this;
	}

	public void UnlockLobbyDoor() {
		StartCoroutine(OpenLobbyDoorCoroutine(leftDoor, rightDoor));
	}

	IEnumerator OpenLobbyDoorCoroutine(GameObject left, GameObject right) {
		Vector3 startLeft = left.transform.position;
		Vector3 startRight = right.transform.position;

		Vector3 targetLeft = startLeft + left.transform.right * doorOpenDistance;
		Vector3 targetRight = startRight - right.transform.right * doorOpenDistance;

		float t = 0;
		while (t < doorOpenTime) {
			float percent = t / doorOpenTime;
			left.transform.position = Vector3.Lerp(startLeft, targetLeft, percent * percent);
			right.transform.position = Vector3.Lerp(startRight, targetRight, percent * percent);

			if (percent * percent > 0.25f) {
				lobbyPortal.GetComponent<BoxCollider>().isTrigger = true;
			}

			t += Time.deltaTime;
			yield return 0;
		}
	}
}
