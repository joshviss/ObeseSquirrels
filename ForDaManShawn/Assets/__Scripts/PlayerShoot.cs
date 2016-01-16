using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShoot : MonoBehaviour
{
    GameObject target;
    Camera playerCamera;
    RaycastHit hitInfo;
    Collider areaOfEffect;
    LineRenderer laser;
    List<Vector3> laserPoints;
    public bool lockedOn;
    public int pointCount;
	public float rayDistance = 50;
	public bool inCollider;

    // Use this for initialization
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        areaOfEffect = GetComponentInChildren<Collider>();
        laser = GetComponent<LineRenderer>();
        laser.SetVertexCount(pointCount);
		laserPoints = new List<Vector3>();
        lockedOn = false;
		inCollider = false;
    }

    void renderLaser(GameObject target)
    {
        laserPoints.Clear();
        Vector3 positionDiff = target.transform.position - gameObject.transform.position;
        //amount z will change per point
        float zChange = positionDiff.z / pointCount;

        //the "laser" will curve only on the x and y axis
        float yDiff = positionDiff.y;
        float xDiff = positionDiff.x;
        float yChange, xChange, pointValRatio;

        for (int i = 0; i < pointCount; i++)
        {
            pointValRatio = ((2 * (i + 1)) / (pointCount * (pointCount + 1)));
            yChange = yDiff * pointValRatio;
            xChange = xDiff * pointValRatio;
            Vector3 pointPosition = new Vector3(xChange, yChange, zChange) + gameObject.transform.position;
            laser.SetPosition(i, pointPosition);
            laserPoints.Add(pointPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lockedOn)
        {
            renderLaser(target);
        }
        if (Input.GetMouseButton(0) && (!lockedOn || inCollider))
        {
			Debug.DrawRay(gameObject.transform.position, playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)).direction * rayDistance, Color.blue, 0, true);
            if (Physics.Raycast(gameObject.transform.position, playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)).direction, out hitInfo, rayDistance, ~(1 << LayerMask.NameToLayer("Player"))))
            {
				print("DRRRRRURURURURURURURURU");
				if (hitInfo.collider.gameObject.tag == "Enemy") {
					lockedOn = true;
					target = hitInfo.collider.gameObject;
				}
            }
        }
        else
        {
            lockedOn = false;
        }
    }

	void OnTriggerStay(Collider other) {
		if (target != null) {
			if (other.gameObject == target)
				inCollider = true;
		}	
	}

	void OnTriggerExit(Collider other) {
		if (target != null) {
			if (other.gameObject == target) {
				inCollider = false;
				target = null;
			}
		}
	}
}