using UnityEngine;
using System.Collections;

public class PortalCam : MonoBehaviour {
    public Camera cam;

    public Transform portal1;
    public Camera portal1Cam;

    public Transform portal2;
    public Camera portal2Cam;

    public bool bob;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion q = Quaternion.FromToRotation(-portal1.up, cam.transform.forward);
        portal1Cam.transform.position = portal2.position + (cam.transform.position - portal1.position);
        portal1Cam.transform.LookAt(portal1Cam.transform.position + q * portal2.up, portal2.transform.forward);
        Vector3 p1eulerAngles = portal1Cam.transform.rotation.eulerAngles;
        portal1Cam.transform.rotation = Quaternion.Euler(p1eulerAngles.x, p1eulerAngles.y + 90, p1eulerAngles.z + 180);
        portal1Cam.nearClipPlane = (portal1Cam.transform.position - portal2.position).magnitude - 0.3f;

        q = Quaternion.FromToRotation(-portal2.up, cam.transform.forward);
        portal2Cam.transform.position = portal1.position + (cam.transform.position - portal2.position);
        portal2Cam.transform.LookAt(portal2Cam.transform.position + q * portal1.up, portal1.transform.forward);
        Vector3 p2eulerAngles = portal2Cam.transform.rotation.eulerAngles;
        portal1Cam.transform.rotation = Quaternion.Euler(p2eulerAngles.x, p2eulerAngles.y + 90, p2eulerAngles.z + 180);
        portal2Cam.nearClipPlane = (portal2Cam.transform.position - portal1.position).magnitude - 0.3f;
    }
}
