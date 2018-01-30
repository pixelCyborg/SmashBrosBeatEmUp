using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowUpgrade : MonoBehaviour {
    public Crossbow crossbow;

    private void Start()
    {
        crossbow = GetComponent<Crossbow>();
    }

    internal virtual void ApplyUpgrade() { }
    internal virtual void OnReload() { }
    internal virtual void OnShoot(float x, float y) { }
}