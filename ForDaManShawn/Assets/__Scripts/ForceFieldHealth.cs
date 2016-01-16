using UnityEngine;
using System.Collections;

public class ForceFieldHealth : MonoBehaviour {

    public int curHealth;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (curHealth <= 0)
            Destroy(gameObject);
    }
}
