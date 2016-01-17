using UnityEngine;
using System.Collections;

public class Forcefield : MonoBehaviour {
	public float startingHealth = 100f;
	public float health;
	float healthLastFrame;

	ParticleSystem barrier;
	public Gradient barrierHealthColor;

	// Use this for initialization
	void Start () {
		healthLastFrame = health;
		health = startingHealth;
		barrier = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug health draining
		//health -= 10 * Time.deltaTime;

		if (healthLastFrame != health) {
			healthLastFrame = health;
			UpdateBarrierColor();
		}
	}

	void UpdateBarrierColor() {
		barrier.startColor = barrierHealthColor.Evaluate(1 - (health / startingHealth));

		//If the barrier is destroyed, turn off the particle system and the particle system
		if (health < 0) {
			GetComponentInChildren<SphereCollider>().enabled = false;
			barrier.Stop();
            SoundManager.myInstance.Play("Forcefield_Down");
            print("Playing sound!");
		}
	}
}
