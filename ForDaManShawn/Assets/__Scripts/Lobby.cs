using UnityEngine;
using System.Collections;

public class Lobby : MonoBehaviour {
	public static Lobby S;
	
	public GameObject hexTilePrefab;        //Grabbed from the inspector

	float inCircleRadius = 2* 0.43301270189f;  //Radius of the circle that fits within a hexagon tile (used to tesselate appropriately)
	float outCircleRadius = 1f;           //Radius of the circle that fits around the hexagon tile

	public Transform floor;
	public Transform ceiling;

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

		BuildLobbyFloorAndWalls();
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


	void BuildLobbyFloorAndWalls() {
		int size = 12;
		int height = 12;

		//Build floor
		floor.position = transform.position + (floor.GetComponent<MeshCollider>().bounds.extents / 2f) + Vector3.up;
		ceiling.position = floor.position + Vector3.up * (height - 1);

		floor.gameObject.GetComponent<MeshRenderer>().enabled = false;
		ceiling.gameObject.GetComponent<MeshRenderer>().enabled = false;

		//Tessellate the hex tiles according to the grid size specified as arguments
		for (int z = 0; z < size; z++) {
			for (int x = 0; x < size; x++) {
				//Spaces the tiles out for tesselation
				float targetX = transform.position.x + 1.5f * (float)x * outCircleRadius;
				float targetZ = transform.position.z + 2 * (float)z * inCircleRadius + (x%2 * inCircleRadius);
				float targetY = transform.position.y + 0;

				GameObject newHexGO = Instantiate(hexTilePrefab, new Vector3(targetX, targetY, targetZ), new Quaternion()) as GameObject;
				newHexGO.transform.SetParent(transform);
				newHexGO.GetComponent<MeshCollider>().enabled = false;
			}
		}
		//Build ceiling
		for (int z = 0; z < size; z++) {
			for (int x = 0; x < size; x++) {
				//Spaces the tiles out for tesselation
				float targetX = transform.position.x + 1.5f * (float)x * outCircleRadius;
				float targetZ = transform.position.z + 2 * (float)z * inCircleRadius + (x%2 * inCircleRadius);
				float targetY = transform.position.y + height;

				GameObject newHexGO = Instantiate(hexTilePrefab, new Vector3(targetX, targetY, targetZ), new Quaternion()) as GameObject;
				newHexGO.transform.SetParent(transform);
				newHexGO.GetComponent<MeshCollider>().enabled = false;
			}
		}

		//Build walls
		//-Z wall
		for (int y = 0; y < height; y++) {
			for (int x = -1; x < size; x++) {
				//Spaces the tiles out for tesselation
				float targetX = transform.position.x + 1.5f * (float)x * outCircleRadius;
				float targetZ = transform.position.z - 2 * inCircleRadius + (Mathf.Abs(x)%2 * inCircleRadius);
				float targetY = transform.position.y + y;

				GameObject newHexGO = Instantiate(hexTilePrefab, new Vector3(targetX, targetY, targetZ), new Quaternion()) as GameObject;
				newHexGO.transform.SetParent(transform);
			}
		}
		//+Z wall
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < size; x++) {
				//Spaces the tiles out for tesselation
				float targetX = transform.position.x + 1.5f * (float)x * outCircleRadius;
				float targetZ = transform.position.z + 2 * size * inCircleRadius + (x%2 * inCircleRadius);
				float targetY = transform.position.y + y;

				GameObject newHexGO = Instantiate(hexTilePrefab, new Vector3(targetX, targetY, targetZ), new Quaternion()) as GameObject;
				newHexGO.transform.SetParent(transform);
			}
		}

		//-X wall
		for (int y = 0; y < height; y++) {
			for (int z = 0; z <= size; z++) {
				//Spaces the tiles out for tesselation
				float targetX = transform.position.x - 1.5f * outCircleRadius;
				float targetZ = transform.position.z + 2 * (float)z * inCircleRadius + (1%2 * inCircleRadius);
				float targetY = transform.position.y + y;

				GameObject newHexGO = Instantiate(hexTilePrefab, new Vector3(targetX, targetY, targetZ), new Quaternion()) as GameObject;
				newHexGO.transform.SetParent(transform);
			}
		}
		//+X wall
		for (int y = 0; y < height; y++) {
			for (int z = 0; z <= size; z++) {
				//Spaces the tiles out for tesselation
				float targetX = transform.position.x + size * 1.5f * outCircleRadius;
				float targetZ = transform.position.z + 2 * (float)z * inCircleRadius + (size%2 * inCircleRadius);
				float targetY = transform.position.y + y;

				GameObject newHexGO = Instantiate(hexTilePrefab, new Vector3(targetX, targetY, targetZ), new Quaternion()) as GameObject;
				newHexGO.transform.SetParent(transform);
			}
		}
	}
}
