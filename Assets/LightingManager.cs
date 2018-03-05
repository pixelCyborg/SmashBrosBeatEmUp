using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightingManager : MonoBehaviour {
    public static LightingManager instance;
    public GameObject lightPrefab;

    private Transform cam;
    public SpriteRenderer darkness;
    public SpriteRenderer totalDarkness;
    public SpriteRenderer dimness;

    private void Awake()
    {
        instance = this;

        //dimness.enabled = true;
        //darkness.enabled = true;
        //totalDarkness.enabled = true;
        cam = Camera.main.transform;
    }

    private void Start()
    {
        if (FindObjectOfType<MapGenerator>() != null) ToggleDarkness(true);
    }

    public void ToggleDarkness(bool active)
    {
        darkness.enabled = active;
        totalDarkness.enabled = active;
        dimness.enabled = active;
    }

    private void Update()
    {
        Vector3 pos = cam.position;
        pos.z = 0;
        dimness.transform.position = pos;
        darkness.transform.position = pos;
        totalDarkness.transform.position = pos;
    }

    public LightSource CreateLightSource(Transform origin, float radius = 5.0f, Color tint = default(Color), LightSource.Strength strength = LightSource.Strength.Full)
    {
        LightSource source = Instantiate(lightPrefab, transform).GetComponent<LightSource>();
        source.origin = origin;
        source.radius = radius;
        source.strength = strength;
        source.tint = tint;
        source.Initialize();
        return source;
    }
}
