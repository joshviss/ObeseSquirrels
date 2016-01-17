using UnityEngine;
using System.Collections;

public class PortalCameras : MonoBehaviour {
	public Vector3 startPos;
	Transform player;
	public Transform renderTexture;
	public Renderer cameraRenderer;
	Camera thisCamera;

	float minFOV = 40;
	float maxFOV = 120;

	float maxFOVChangeDistance = 5;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		thisCamera = GetComponent<Camera>();
		player = GameManager.S.playerMovement.gameObject.transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//transform.position = startPos;

		if (thisCamera == null) {
			return;
		}
		if (renderTexture != null) {
			thisCamera.enabled = cameraRenderer.gameObject.GetComponent<Renderer>().isVisible;

			//Update field of view depending on the distance between the player and the renderTexture
			float distance = (player.position - renderTexture.position).magnitude;
			float percent = 1-distance / maxFOVChangeDistance;
			//print(percent);
			thisCamera.fieldOfView = Mathf.Lerp(minFOV, maxFOV, Mathf.Sign(percent)*(percent*percent));
			transform.position = startPos - transform.forward * distance;
			thisCamera.nearClipPlane = distance;
		}
	}
}
