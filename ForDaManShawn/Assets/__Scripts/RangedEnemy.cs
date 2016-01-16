using UnityEngine;
using System.Collections;

public class RangedEnemy : MonoBehaviour {
	Vector3 centerOfMovement;

	float figureEightHeight = 0.5f;
	float figureEightWidth = 1.25f;

	// Use this for initialization
	void Start () {
		centerOfMovement = transform.position;
		StartFigureEightMovement();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void StartFigureEightMovement() {
		StartCoroutine(FigureEightMovementCoroutine());
	}
	IEnumerator FigureEightMovementCoroutine() {
		float t = 0;

		while (true) {
			transform.position = centerOfMovement + new Vector3(figureEightWidth*Mathf.Sin(Mathf.PI * t / 4f), 
																figureEightHeight*Mathf.Sin(Mathf.PI * t / 2f), 
																figureEightWidth * Mathf.Sin(Mathf.PI * t / 43));

			t += Time.deltaTime;
			yield return 0;
		}
	}
}
