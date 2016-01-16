using UnityEngine;
using System.Collections;

public class EnemyCircleMovement : MonoBehaviour {

	//variables for circular movement
	public float speed = 5f;
	public float radius = 2.5f;
	public Vector3 startDirection = new Vector3(0, 1, 0);
	public Vector3 ninetyDegDirection = new Vector3(1, 0, 0);

	Vector3 pathOrigin, startLocation;
	bool circleMoving;
	float radianSpeed, moveStartTime;

	// Use this for initialization
	void Start () {
		//used for when doing circular movement
		pathOrigin = transform.position;
		circleMoving = false;

		if (startDirection.magnitude == 0) {
			startDirection = new Vector3(0, 1, 0);
		}
		startDirection.Normalize();

		if (ninetyDegDirection.magnitude == 0) {
			startDirection = new Vector3(1, 0, 0);
		}
		ninetyDegDirection.Normalize();

		radianSpeed = speed / (2 * Mathf.PI);
	}

	//for circular movement patrolling
	IEnumerator circlePatrol() {
		float timeIn, percentComp;

		moveStartTime = Time.time;
		percentComp = 0;

		while (percentComp < 1) {
			//checks how long this function has been running
			timeIn = Time.time - moveStartTime;
			//calculates percent of patrol distance traveled
			percentComp = (timeIn * radianSpeed) / (2 * Mathf.PI);
			transform.position = Mathf.Cos(timeIn * radianSpeed)*startDirection * radius 
				+ Mathf.Sin(timeIn * radianSpeed) * ninetyDegDirection * radius + pathOrigin;

			//fixes the percentComp if goes over 1
			if (percentComp > 1) {
				percentComp = 1;
			}

			//checks every fixed frame
			yield return new WaitForFixedUpdate();
		}

		circleMoving = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!circleMoving) {
			circleMoving = true;
			StartCoroutine("circlePatrol");
		}
	}
}
