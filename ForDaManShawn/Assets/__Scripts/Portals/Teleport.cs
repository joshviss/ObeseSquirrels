using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {
	public PortalCameras target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag != "Player") {
			return;
		}

		other.gameObject.transform.position = new Vector3(target.startPos.x, other.gameObject.transform.position.y, target.startPos.z);
    }
}
