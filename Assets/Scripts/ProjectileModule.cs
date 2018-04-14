﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProjectileModule : MonoBehaviour {
    public int numTestProjectiles = 1;
    public float currentRotation = 0;
    public float currentScale = 5;
    public float rotationSpeed = 2;
    public float projectileSize;

    public GameObject spectralFireball = null;
    public List<Projectile> projectiles = new List<Projectile>();

    private void Start()
    {
        TestHalo();
    }

    private void Update()
    {
        currentRotation += Time.deltaTime * rotationSpeed;
        UpdateOrbit();
    }

    private void SpawnProjectile()
    {
        GameObject fireball = Instantiate(spectralFireball, transform.position, Quaternion.identity);
        fireball.GetComponent<Rigidbody2D>().isKinematic = true;
        fireball.transform.localScale = Vector3.one * projectileSize;
        projectiles.Add(fireball.GetComponent<Projectile>());
    }

    private void UpdateOrbit()
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            float rotPos = (360f / (float)projectiles.Count) * i;
            Vector3 displacement = Quaternion.AngleAxis(rotPos, Vector3.forward) * Vector3.up;
            displacement = Quaternion.AngleAxis(currentRotation, Vector3.forward) * displacement;
            projectiles[i].transform.position = transform.position + (displacement * currentScale);
        }
    }

    void ClearHalo()
    {
        if (projectiles.Count > 0)
        {
            Debug.Log(projectiles.Count);
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                if (projectiles[i] != null)
                {
                    DestroyImmediate(projectiles[i].gameObject);
                }
            }
        }

        projectiles = new List<Projectile>();
    }

    public void TestHalo()
    {
        ClearHalo();

        for (int i = 0; i < numTestProjectiles; i++)
        {
            SpawnProjectile();
        }

        UpdateOrbit();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ProjectileModule))]
public class ProjectileModuleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ProjectileModule myScript = (ProjectileModule)target;
        if (GUILayout.Button("Generate Halo"))
        {
            myScript.TestHalo();
        }
    }
}
#endif
