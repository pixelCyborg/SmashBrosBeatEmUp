using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multishot : CrossbowUpgrade
{
    internal override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
    }

    internal override void OnShoot(float x, float y)
    {
        base.OnShoot(x, y);
        Vector2 shootVector = new Vector2(x, y);
        Vector2 upVector = Rotate(shootVector, -15);
        Vector2 downVector = Rotate(shootVector, 15);

        crossbow.Shoot(upVector.x, upVector.y);
        crossbow.Shoot(downVector.x, downVector.y);
    }

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
