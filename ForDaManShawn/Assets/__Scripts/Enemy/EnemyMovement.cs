using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	//speed variables
	public float speed = 5f;
	public float waitTime = 1f;
	public Vector3 patrolPoint = new Vector3(5, 3, 4);

	Vector3 startPoint, curPoint;
	bool patrolling, toPatrolPoint;
	float patrolStartTime, patrolDistance;

	// Use this for initialization
	void Awake() {
		startPoint = transform.position;
		patrolling = false;
		toPatrolPoint = true;
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
			transform.position = Vector3.Lerp(curPoint, patrolPoint, percentComp);

			//checks every fixed frame
			yield return new WaitForFixedUpdate();
		}

		toPatrolPoint = false;

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
			transform.position = Vector3.Lerp(curPoint, startPoint, percentComp);

			//checks every fixed frame
			yield return new WaitForFixedUpdate();
		}

		toPatrolPoint = true;

		StartCoroutine("patrolWait");
	}

	IEnumerator patrolWait() {
		yield return new WaitForSeconds(waitTime);
		patrolling = false;
	}

	// Update is called once per frame
	void Update () {
		//*********only need this below for patrolling ***********
		if (!patrolling) {
			patrolling = true;
			//gets the current position to start patrol
			curPoint = transform.position;
			if (toPatrolPoint) { //patrols to patrol point
				StartCoroutine("goToPP");
			} else { //patrols to start point
				StartCoroutine("goToSP");
			}
		}

	}

	void FixedUpdate() {

	}

	
}
