using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {
	enum KeyState {
		notPickedUp,
		onPlayer,
		inLobby
	}
	KeyState state = KeyState.notPickedUp;

	float rotationSpeed = 1f;       //How fast the key orbits the player
	float orbitRadius = 1.8f;         //How far away from the player the key orbits
	float orbitHeight = 0.9f;		//How far above the center of the player the key orbits

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Do nothing while waiting to be picked up
		if (state == KeyState.notPickedUp) {
			return;
		}
		//If the key is on the player, orbit around the player
		else if (state == KeyState.onPlayer) {
			OrbitPlayer();
		}
	}

	void OnTriggerEnter(Collider other) {
		//Ignore collisions when key is already picked up or by not the player
		if (state != KeyState.notPickedUp || other.gameObject.tag != "Player") {
			return;
		}

		//If the player picked up this key, update the state to reflect it
		state = KeyState.onPlayer;
	}

	void OrbitPlayer() {
		StartCoroutine(OrbitPlayerCoroutine());
	}
	IEnumerator OrbitPlayerCoroutine() {
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
	}
}
