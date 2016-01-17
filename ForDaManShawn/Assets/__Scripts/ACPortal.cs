using UnityEngine;
using System.Collections;

public class ACPortal : MonoBehaviour {
	public Transform endPortal;

	// Use this for initialization
	void Start () {
		endPortal = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		print("Collided with " + other.gameObject.name);
		if (other.gameObject.tag != "Player") {
			return;
		}

		Vector3 offset = other.gameObject.transform.position - transform.position;
		
		other.gameObject.transform.position = endPortal.position + offset;
		PlayerAim aim = other.gameObject.GetComponentInChildren<PlayerAim>();
		aim.portalX += transform.rotation.eulerAngles.x + endPortal.rotation.eulerAngles.x;
		aim.portalY += transform.rotation.eulerAngles.y + endPortal.rotation.eulerAngles.y;
		aim.portalZ += transform.rotation.eulerAngles.z + endPortal.rotation.eulerAngles.z;
		
		float angleDiff = Mathf.Abs(transform.rotation.eulerAngles.y - endPortal.rotation.eulerAngles.y);
		//Don't even ask
		if (gameObject.name == "ACPortalF") {
			angleDiff = 0;
		}
		if (Mathf.Abs(angleDiff - 180) < 5f) {
			Rigidbody playerRigidbody = aim.gameObject.GetComponentInParent<Rigidbody>();
			Vector3 curVel = playerRigidbody.velocity;
			print(playerRigidbody.velocity);
			playerRigidbody.velocity = new Vector3(-curVel.x, curVel.y, -curVel.z);		
		}
		else if (Mathf.Abs(angleDiff - 90) < 5f) {
			Rigidbody playerRigidbody = aim.gameObject.GetComponentInParent<Rigidbody>();
			Vector3 curVel = playerRigidbody.velocity;
			print(playerRigidbody.velocity);
			playerRigidbody.velocity = new Vector3(curVel.z, curVel.y, curVel.x);
		}
		else if (Mathf.Abs(angleDiff - 270) < 5f) {
			Rigidbody playerRigidbody = aim.gameObject.GetComponentInParent<Rigidbody>();
			Vector3 curVel = playerRigidbody.velocity;
			print(playerRigidbody.velocity);
			playerRigidbody.velocity = new Vector3(-curVel.z, curVel.y, -curVel.x);
		}
	}
}
