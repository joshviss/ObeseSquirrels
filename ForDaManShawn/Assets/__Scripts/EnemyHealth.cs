using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int curHealth;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (curHealth <= 0) {
			SoundManager.myInstance.Play("Collectable_Get");
			Destroy(gameObject);
		}
	}
}
