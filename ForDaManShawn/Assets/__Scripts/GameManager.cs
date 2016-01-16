using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager S;    //Singleton reference

	public PlayerMovement playerMovement;

	// Use this for initialization
	void Start () {
		S = this;
		playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
