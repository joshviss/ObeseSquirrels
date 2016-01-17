using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {
	public enum KeyState {
		notPickedUp,
		onPlayer,
		inLobby,
		inLock
	}
	public KeyState state = KeyState.notPickedUp;

	float rotationSpeed = 1f;       //How fast the key orbits the player
	float orbitRadius = 1.8f;         //How far away from the player the key orbits
	float orbitHeight = 0.9f;       //How far above the center of the player the key orbits

	float enterLockTime = 5f;       //How many seconds it takes for the key to enter the lock
	float keyTurnTime = 1f;			//How many seconds it takes for the key to turn in the lock

	bool inOrbitCoroutine = false;
	bool inEnterLockCoroutine = false;
	bool inSpinInPlaceCoroutine = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Spin in place while waiting to be picked up
		if (state == KeyState.notPickedUp) {
			SpinInPlace();
			return;
		}
		//If the key is on the player, orbit around the player
		else if (state == KeyState.onPlayer) {
			OrbitPlayer();
		}
		else if (state == KeyState.inLobby) {
			EnterLock();
		}
	}

	void OnTriggerEnter(Collider other) {
        //If the player picked up this key, update the state to reflect it
		if (state == KeyState.notPickedUp && other.gameObject.tag == "Player") {
			state = KeyState.onPlayer;
			other.gameObject.GetComponent<PlayerMovement>().keyOnPlayer = this;
            SoundManager.myInstance.Play("Key_Get");
			return;
		}

	}

	void SpinInPlace() {
		if (!inSpinInPlaceCoroutine) {
			StartCoroutine(SpinInPlaceCoroutine());
		}
	}
	IEnumerator SpinInPlaceCoroutine() {
		inSpinInPlaceCoroutine = true;

		while (state == KeyState.notPickedUp) {
			Vector3 curEulerAngles = transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler(curEulerAngles.x, curEulerAngles.y + 90*Time.deltaTime * rotationSpeed, curEulerAngles.z);

			yield return 0;
		}

		inSpinInPlaceCoroutine = false;
	}

	void OrbitPlayer() {
		if (!inOrbitCoroutine) {
			StartCoroutine(OrbitPlayerCoroutine());
		}
	}
	IEnumerator OrbitPlayerCoroutine() {
		inOrbitCoroutine = true;
		float t = 0;	//Time passed since entering coroutine
		while (state == KeyState.onPlayer) {
			Vector3 curEulerRot = transform.eulerAngles;
			curEulerRot.z = 0;
			transform.eulerAngles = curEulerRot;

			Vector3 playerPos = GameManager.S.playerMovement.transform.position;
			Vector3 keyPos = new Vector3(orbitRadius * Mathf.Cos(rotationSpeed * t), orbitHeight, orbitRadius * Mathf.Sin(rotationSpeed * t));

			transform.position = playerPos + keyPos;
			transform.LookAt(playerPos + 4*Vector3.up);

			t += Time.deltaTime;

			yield return 0;
		}
		inOrbitCoroutine = false;
	}

	void EnterLock() {
		if (!inEnterLockCoroutine) {
			StartCoroutine(EnterLockCoroutine());
		}
	}
	IEnumerator EnterLockCoroutine() {
		inEnterLockCoroutine = true;
		//Find the appropriate lock in the game world
		string targetLockName = "";
		switch (gameObject.name) {
			case "ACKey":
				targetLockName = "ACLock";
				break;
			case "FutureKey":
				targetLockName = "FutureLock";
				break;
			case "NatureKey":
				targetLockName = "NatureLock";
				break;
		}
		GameObject lockGO = GameObject.Find(targetLockName);
		Vector3 lockPos = lockGO.transform.position + 0.5f*lockGO.transform.up;
		Quaternion lockRot = Quaternion.Euler(lockGO.transform.rotation.eulerAngles + 90*Vector3.up);
		Quaternion turnRot = Quaternion.Euler(lockRot.eulerAngles + 90*Vector3.right);

		Vector3 startPos = transform.position;
		Quaternion startRot = transform.rotation;

		print(lockPos);

		float t = 0;
		while (state != KeyState.inLock) {
			float percent = t / enterLockTime;

			transform.position = Vector3.Lerp(startPos, lockPos, percent);
			transform.rotation = Quaternion.Lerp(startRot, lockRot, percent);

			t += Time.deltaTime;

			if (t > enterLockTime) {
				state = KeyState.inLock;
			}
			yield return 0;
		}

		yield return new WaitForSeconds(0.5f);
		print("Turning key");

		//plays the key turning sfx
		SoundManager.myInstance.Play("Key_Unlock");

		//Turn the key. Like Bob Segar. Wait, no that's Turn the Page. Disregard.
		t = 0;
		while (t < keyTurnTime) {
			float percent = t / keyTurnTime;

			transform.rotation = Quaternion.Lerp(lockRot, turnRot, percent);

			t += Time.deltaTime;
			yield return 0;
		}

		Lobby.S.numKeysInLock = Lobby.S.numKeysInLock + 1;

		inEnterLockCoroutine = false;
	}
}
