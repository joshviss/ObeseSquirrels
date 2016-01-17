using UnityEngine;
using System.Collections;

public class ACLevel : MonoBehaviour {
	public static ACLevel S;

	public GameObject infiniteHallwayPortal;
	public GameObject firstWall;
	public GameObject ExitWall;
	public GameObject key;
	public GameObject forcefield;
	EnergyManage playerEnergy;

	bool playerHasAllCollectibles = false;
	bool playerHasOneCollectible = false;

	// Use this for initialization
	void Awake () {
		S = this;
		print("ACLevel pieces used: " + transform.childCount);
	}

	void Start() {
		playerEnergy = GameManager.S.playerMovement.gameObject.GetComponent<EnergyManage>();
		infiniteHallwayPortal.SetActive(!playerHasAllCollectibles);
		ExitWall.SetActive(playerHasAllCollectibles);
		key.SetActive(playerHasAllCollectibles);
		forcefield.SetActive(playerHasAllCollectibles);
	}

	void Update() {
		if (GameManager.S.playerInRoom != GameManager.Room.AC) {
			return;
		}

		if (!playerHasOneCollectible && playerEnergy.playerEnergy >= 100) {
			playerHasOneCollectible = true;
			firstWall.SetActive(false);
		}

		//4 collectibles == 400 energy, fuck code quality
		if (!playerHasAllCollectibles && playerEnergy.playerEnergy >= 400) {
			playerHasAllCollectibles = true;
			infiniteHallwayPortal.SetActive(!playerHasAllCollectibles);
			ExitWall.SetActive(playerHasAllCollectibles);
			key.SetActive(playerHasAllCollectibles);
			forcefield.SetActive(playerHasAllCollectibles);
		}
	}
	
}
