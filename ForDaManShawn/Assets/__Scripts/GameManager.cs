using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager S;    //Singleton reference

	public PlayerMovement playerMovement;
	public enum Room {
		Tutorial,
		Lobby,
		AC,
		Future,
		Nature,
		Final
	}
	public Room playerInRoom = Room.Tutorial;

	// Use this for initialization
	void Awake () {
		S = this;
		playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
