using UnityEngine;
using System.Collections;

public class EnergyManage : MonoBehaviour {

    public int playerEnergy = 0;
    public int energyTransferRate = 1;

    // Use this for initialization
    void Start() {

    }

    public void TransferEnergy(GameObject target) {
        if (target.tag == "Enemy") {
			SoundManager.myInstance.Play("Gun_Shoot_Collectable3");
            playerEnergy += energyTransferRate;
            target.GetComponent<EnemyHealth>().curHealth -= energyTransferRate;
        }
        else if (target.tag == "ForceField") {
			SoundManager.myInstance.Play("Gun_Shoot_Force_Field2");
            playerEnergy -= 2 * energyTransferRate;
            target.GetComponentInParent<Forcefield>().health -= 2 * energyTransferRate;

            if (playerEnergy < 0) playerEnergy = 0;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
