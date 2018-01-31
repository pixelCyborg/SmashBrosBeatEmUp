using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : CrossbowUpgrade {
    internal override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        crossbow.boltSpeed += 20;
        crossbow.damage += 1;
        crossbow.reloadTime += 0.15f;
    }

    internal override void RemoveUpgrade()
    {
        crossbow.boltSpeed -= 20;
        crossbow.damage -= 1;
        crossbow.reloadTime -= 0.15f;

        base.RemoveUpgrade();
    }
}
