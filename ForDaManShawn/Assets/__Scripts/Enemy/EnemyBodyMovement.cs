using UnityEngine;
using System.Collections;

public class EnemyBodyMovement : MonoBehaviour {
	/*
	public enum BobDir {
		Y,
		X,
		Z
	};
	
	public BobDir enemyBob;
	*/
	public float speed = 2f;
	public Vector3 bobDirection = new Vector3(0, 1, 0);
	public float bobDist = 0.5f;
	public float waitTime = 0.2f;

	Vector3 startPoint, patrolPoint, curPoint;
	bool patrolling, startingDirection;
	float patrolStartTime, patrolDistance;

	// Use this for initialization
	void Start () {
		startingDirection = true;
		if(bobDirection.magnitude == 0) {
			bobDirection = new Vector3(0, 1, 0);
		}
		bobDirection.Normalize();

		patrolPoint = (bobDirection * (bobDist / 2)) + transform.localPosition;
		startPoint = (-bobDirection * (bobDist / 2)) + transform.localPosition;
		curPoint = transform.localPosition;

		patrolling = false;
	}

	//the enemy moves to the patrol point
	IEnumerator goToPP() {
		float timeIn, distanceToTravel, percentComp;

		patrolStartTime = Time.time;
		distanceToTravel = Vector3.Distance(curPoint, patrolPoint);
		percentComp = 0;

		while (percentComp < 1) {
			//checks how long this function has been running
			timeIn = Time.time - patrolStartTime;
			//calculates percent of patrol distance traveled
			percentComp = (timeIn * speed) / distanceToTravel;

			//fixes the percentComp if goes over 1
			if (percentComp > 1) {
				percentComp = 1;
			}

			//lerps across the patrol path
			transform.localPosition = Vector3.Lerp(curPoint, patrolPoint, percentComp);

			//checks every fixed frame
			yield return new WaitForFixedUpdate();
		}

		startingDirection = false;

		StartCoroutine("patrolWait");
	}

	//the enemy moves to the start point
	IEnumerator goToSP() {
		float timeIn, distanceToTravel, percentComp;

		patrolStartTime = Time.time;
		distanceToTravel = Vector3.Distance(curPoint, startPoint);
		percentComp = 0;

		while (percentComp < 1) {
			//checks how long this function has been running
			timeIn = Time.time - patrolStartTime;
			//calculates percent of patrol distance traveled
			percentComp = (timeIn * speed) / distanceToTravel;

			//fixes the percentComp if goes over 1
			if (percentComp > 1) {
				percentComp = 1;
			}

			//lerps across the patrol path
			transform.localPosition = Vector3.Lerp(curPoint, startPoint, percentComp);

			//checks every fixed frame
			yield return new WaitForFixedUpdate();
		}

		startingDirection = true;

		StartCoroutine("patrolWait");
	}

	IEnumerator patrolWait() {
		yield return new WaitForSeconds(waitTime);
		patrolling = false;
	}

	// Update is called once per frame
	void Update() {
		if (!patrolling) {

			patrolling = true;
			//gets the current position to start patrol
			curPoint = transform.localPosition;
			//debug
			print(curPoint);
			if (startingDirection) { //patrols to patrol point
				StartCoroutine("goToPP");
			} else { //patrols to start point
				StartCoroutine("goToSP");
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		/* Decided to do it a different way
		switch(enemyBob) {
			case BobDir.Y:
				if (startingDirection) { }
				break;
			case BobDir.X:

				break;
			case BobDir.Z:

				break;
			default:
				print("Error with EnemyBodyMovement");
				break;
		}
		*/
	}
}
