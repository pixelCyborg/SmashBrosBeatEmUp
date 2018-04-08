using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowUpgrade : MonoBehaviour {
    public Crossbow crossbow;
    public Upgrade upgrade;

    public enum Upgrade
    {
        None, Multishot, Scope, Repeater
    }

    private void Start()
    {
        crossbow = GetComponent<Crossbow>();
    }

    internal virtual void ApplyUpgrade() { crossbow = GetComponent<Crossbow>(); }
    internal virtual void RemoveUpgrade() { Destroy(this); }
    internal virtual void OnReload() { }
    internal virtual void OnShoot(float x, float y) { }
}