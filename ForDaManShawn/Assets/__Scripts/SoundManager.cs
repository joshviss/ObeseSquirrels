using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct Sound_Pair{
    public string key;
    public AudioClip value;
}

public class SoundManager : MonoBehaviour {
    private static SoundManager _myInstance;

    public AudioSource musicChannel1;
    public AudioSource musicChannel2;
    public AudioSource musicChannel3;
    public AudioSource musicChannel4;
    public AudioSource musicChannel5;

    public Sound_Pair[] sounds;
    private Dictionary<string, AudioClip> soundDict;

    public static SoundManager myInstance{
        get {
            if (_myInstance == null) print("OH SHIT, SOUND MANAGER IS NULL");

            return _myInstance;
        }

    }

    void Awake() {
        if (_myInstance == null) {
            _myInstance = this;
        }
        else if (_myInstance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        soundDict = new Dictionary<string, AudioClip>();
        for (int i = 0; i < sounds.Length; i++) {
            soundDict.Add(sounds[i].key, sounds[i].value);
        }
    }

	// Use this for initialization
	void Start () {
        
	}
	
    public void Play(string soundName) {
        int channel = getChannel(soundName);
        
        switch (channel) {
            case 1:
                musicChannel1.clip = soundDict[soundName];
                musicChannel1.pitch = 1;
                musicChannel1.Play();
                break;
            case 2:
                musicChannel2.clip = soundDict[soundName];
                musicChannel2.pitch = 1;
                musicChannel2.Play();
                break;
            case 3:
                musicChannel3.clip = soundDict[soundName];
                musicChannel3.pitch = 1;
                musicChannel3.Play();
                break;
            case 4:
                musicChannel4.clip = soundDict[soundName];
                musicChannel4.pitch = 1;
                musicChannel4.Play();
                break;
            case 5:
                musicChannel5.clip = soundDict[soundName];
                musicChannel5.pitch = 1;
                musicChannel5.Play();
                break;
            default:
                Debug.LogError("Nah man, \"" + soundName + "\" doesn't exist.");
                break;
        }
    }

    public AudioSource GetSound(string soundName) {
        int channel = getChannel(soundName);

        switch (channel) {
            case 1:
                return musicChannel1;
            case 2:
                return musicChannel2;
            case 3:
                return musicChannel3;
            case 4:
                return musicChannel4;
            case 5:
                return musicChannel5;
            default:
                Debug.LogError("Nah man, \"" + soundName + "\" doesn't exist.");
                return null;
        }
    }

    public int getChannel(string soundName) {
        if (soundName == "lobby")
            return 1;
        if (soundName == "room1")
            return 2;
        if (soundName == "room2")
            return 3;
        if (soundName == "room3")
            return 4;
        if (soundName == "finalRoom")
            return 5;

        else return 0;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
