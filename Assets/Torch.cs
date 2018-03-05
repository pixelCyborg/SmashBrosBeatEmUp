using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {
    public float radius = 5.0f;
    public Color tint = Color.clear;
    public LightSource.Strength strength = LightSource.Strength.Full;

	// Use this for initialization
	void Start () {
        LightingManager.instance.CreateLightSource(transform, radius, tint, strength);	
	}
}
