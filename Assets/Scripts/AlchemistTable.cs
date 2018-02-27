using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistTable : Interactable {

    internal override void OnInteract()
    {
        base.OnInteract();
        Inventory.instance.Toggle();
    }
}
