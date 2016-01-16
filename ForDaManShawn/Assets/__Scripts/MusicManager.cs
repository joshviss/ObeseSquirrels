using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public float framesTilFade;
    AudioSource curTrack;
    bool transitioning;

	// Use this for initialization
	void Start () {
        transitioning = false;

        SoundManager.myInstance.Play("lobby");
        curTrack = SoundManager.myInstance.GetSound("lobby");
        SoundManager.myInstance.Play("room1");
        SoundManager.myInstance.GetSound("room1").volume = 0;
        SoundManager.myInstance.Play("room2");
        SoundManager.myInstance.GetSound("room2").volume = 0;
        SoundManager.myInstance.Play("room3");
        SoundManager.myInstance.GetSound("room3").volume = 0;
        SoundManager.myInstance.Play("finalRoom");
        SoundManager.myInstance.GetSound("finalRoom").volume = 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "musicTrigger") {
            if (curTrack != SoundManager.myInstance.GetSound(other.gameObject.GetComponent<MusicTrigger>().musicTrack)) {
                transitioning = true;
                StartCoroutine(FadeTracks(curTrack, SoundManager.myInstance.GetSound(other.gameObject.GetComponent<MusicTrigger>().musicTrack)));
            }
        }
    }

    IEnumerator FadeTracks(AudioSource track1, AudioSource track2) {
        for (float i = 0; i <= framesTilFade; i++) {
            track1.volume = (framesTilFade - i) / framesTilFade;
            track2.volume = i / framesTilFade;

            yield return null;
        }
        curTrack = track2;
        transitioning = false;
    }
}
