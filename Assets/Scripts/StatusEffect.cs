using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour {
	internal Player player;
	internal NPC npc;

	public float power;

	void Start() {
		player = GetComponent<Player> ();
		npc = GetComponent<NPC> ();
	}
}

//FIRE
public class Burning : StatusEffect {

	void Begin() {
		StartCoroutine (Burn ());
	}

	IEnumerator Burn() {
		while (gameObject.activeSelf) {
			Tick ();
			yield return new WaitForSeconds (1.0f);
		}
	}

	void Tick() {
		if(player != null) {
			player.TakeDamage ((int)power);
		}

		if (npc != null) {
			npc.TakeDamage ((int)power, transform.position);
		}
	}
}
	
//ICE
public class Ice : StatusEffect {

}

//HOLY
public class Holy : StatusEffect {

}