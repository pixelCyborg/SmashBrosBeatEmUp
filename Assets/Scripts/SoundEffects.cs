using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour {
    private static AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();	
	}

    public static void PlaySFX(AudioClip clip, float volume = 1.0f)
    {
        source.PlayOneShot(clip, volume);
    }
}
