using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class Potion {
    public float throwForce = 10.0f;
    public float blastRadius = 1.0f;
    public float damage = 1.0f;
    public Property[] properties;

    public Potion()
    {
        properties = new Property[0];
    }

	public void ApplyStatus(Enemy npc) {
		for (int i = 0; i < properties.Length; i++) {
			switch (properties [i].type) {
			case Property.Type.Fire:
				npc.gameObject.AddComponent<Burning> ();
				break;

			case Property.Type.Ice:
				npc.gameObject.AddComponent<Frozen> ();
				break;
			}
		}
	}

    public void SetStats(Potion stats)
    {
        if (stats == null) return;
        throwForce = stats.throwForce;
        blastRadius = stats.blastRadius;
        damage = stats.damage;
        properties = stats.properties;
    }
}

[System.Serializable]
public class Property
{
    public enum Type
    {
        //===============Physical================
        Bouncy,
        //Makes Potions ricochet
        //Can make surfaces bouncy
        Impact,
        //Will apply extra force to characters hit by the blast
        //Can be used for a double jump
        Puddle,
        //Creates a surface of whatever element the potion is

        //=================Elements==================
        //Damage over time
        Fire,
        //Freezes/Slows Enemies
        //Slippery surfaces(?)
        Ice,
        //Heals living, damages undead
        Holy
    }

    public int power;
    public Type type;

    public Property(Type _type, int _power = 1)
    {
        type = _type;
        power = _power;
    }
}
