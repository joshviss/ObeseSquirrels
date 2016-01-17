using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {
	public GameManager.Room targetRoom;
	public PortalCameras target;
    public static bool triggered = false;
    float cdTimer = 1f;
    float timer;
    Vector3 posOffset = new Vector3 (0.0f, -0.75f, 0.0f);
    Vector3 angleOffset = new Vector3 (0.0f, 0.0f, 0.0f);
    Quaternion objectRot;
    Vector3 finalRot;

    public Transform endPortal;
    void Start () {
        timer = cdTimer;
        finalRot = endPortal.rotation.eulerAngles + angleOffset;
        objectRot = Quaternion.Euler(finalRot.x, finalRot.y, finalRot.z);
    }

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag != "Player") {
			return;
		}

		other.gameObject.transform.position = new Vector3(target.startPos.x, other.gameObject.transform.position.y, target.startPos.z);
		GameManager.S.playerInRoom = targetRoom;
	}

    void Update () {
        if (triggered) {
            print(triggered);
            timer -= Time.deltaTime;
            if (timer <= 0) {
                triggered = false;
                timer = cdTimer;
                print(triggered);
            }
        }
    }
 //   void OnCollisionEnter (Collision collision) {
 //       if (!triggered) {
 //           print(collision.collider.name);
 //           triggered = true;
 //           collision.transform.position = endPortal.position + posOffset;
 //           //            Quaternion curRot = collision.transform.rotation;
 //           //            collision.transform.Rotate(finalRot.x, finalRot.y, finalRot.z, Space.World);
 //           //            collision.transform.rotation = curRot * objectRot;

 //           if (collision.collider.name == "Player") {
 //               Camera portCam = endPortal.GetChild(0).GetComponent<Camera>();
 //               var portRot = portCam.transform.rotation.eulerAngles;
 //               var tempRot = collision.collider.transform.rotation.eulerAngles;
 
 //               var temp = collision.collider.transform.GetChild(0);
 //               PlayerAim aim = temp.GetComponent<PlayerAim> ();
 ////               var endRot = endPortal.transform.rotation.eulerAngles;
 //               aim.portalX += portRot.x;
 //               aim.portalY += -portRot.y;
 //               aim.portalZ += portRot.z;
 //           }
 //       }
 //   }
}
