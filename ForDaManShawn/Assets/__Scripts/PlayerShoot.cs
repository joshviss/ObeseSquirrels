﻿using UnityEngine;
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
    public bool buttonPressed;
    public int pointCount;
	public float rayDistance = 20;
	public bool inCollider;

    EnergyManage energyManager;

    public Material shootEnemy, shootForceField;

    // Use this for initialization
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        areaOfEffect = GetComponentInChildren<Collider>();
        energyManager = GetComponent<EnergyManage>();
        laser = GetComponent<LineRenderer>();
        laser.SetVertexCount(pointCount);
		laserPoints = new List<Vector3>();
        lockedOn = false;
        buttonPressed = false;
		inCollider = false;

		laser.enabled = false;
    }

    void renderLaser(GameObject target)
    {
        //Don't render the laser if the target was destroyed
        //or if the player has no energy and is shooting the force field
        if (target == null || (target.tag == "ForceField" && energyManager.playerEnergy <= 0)) {
            lockedOn = false;
            inCollider = false;
            laser.enabled = false;
            buttonPressed = false;
            return;
        }

        //Change laser based on target
        if (target.tag == "ForceField")
            laser.material = shootForceField;
        else
            laser.material = shootEnemy;

		laser.enabled = true;
        laserPoints.Clear();
        Vector3 positionDiff = target.transform.position - gameObject.transform.position;
        //amount z will change per point
        float zChange = positionDiff.z / pointCount;

        //the "laser" will curve only on the x and y axis
        float yDiff = positionDiff.y;
        float xDiff = positionDiff.x;
        float yChange, xChange, pointValRatio;
		Vector3 totalChange = new Vector3(0, 0, 0);

		//renders all the points
        for (int i = 0; i < pointCount; i++)
        {
			//calculates the ratio of total distance a newly rendered point will
			//render in front of the last rendered point
			pointValRatio = ((2 * (i + 1)) / (float)(pointCount * (pointCount + 1)));

			//calculates the changes in x and y position for the rendered point
            yChange = yDiff * pointValRatio;
            xChange = xDiff * pointValRatio;

			//bad efficiency but works (make more efficient if have time)
			//fixes a problem with rendered laser starting away from the
			//character model
			if (i == 0) { //no change in z
				totalChange = totalChange + new Vector3(xChange, yChange, 0);
			} else if (i == 1) { //twice the change in z
				totalChange = totalChange + new Vector3(xChange, yChange, 2 * zChange);
			} else { //normal change in z
				totalChange = totalChange + new Vector3(xChange, yChange, zChange);
			}

			//calculates the position of the new point
            Vector3 pointPosition = totalChange + gameObject.transform.position;

            laser.SetPosition(i, pointPosition);
            laserPoints.Add(pointPosition);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lockedOn)
        {
            renderLaser(target);
        }
        if ((Input.GetMouseButtonDown(0) || (buttonPressed && Input.GetMouseButton(0))) && (!lockedOn || inCollider))
        {
			Debug.DrawRay(gameObject.transform.position, playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)).direction * rayDistance, Color.blue, 0, true);
            if (Physics.SphereCast(gameObject.transform.position, 0.5f, playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)).direction, out hitInfo, rayDistance, ~((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("MusicTrigger")))))
            {
                if (hitInfo.collider.gameObject.tag == "Enemy" || hitInfo.collider.gameObject.tag == "ForceField") {
					lockedOn = true;
                    buttonPressed = true;
                    target = hitInfo.collider.gameObject;
                    energyManager.TransferEnergy(hitInfo.collider.gameObject);
                }
            }
        }
        else
        {
            lockedOn = false;
			laser.enabled = false;
            buttonPressed = false;
        }
    }

	void OnTriggerStay(Collider other) {
		if (target != null) {
            if (other.gameObject == target) {
                inCollider = true;  
            }
		}	
	}

	void OnTriggerExit(Collider other) {
		if (target != null) {
			if (other.gameObject == target) {
				inCollider = false;
				target = null;
				lockedOn = false;
				laser.enabled = false;
                buttonPressed = false;
			}
		}
	}
}