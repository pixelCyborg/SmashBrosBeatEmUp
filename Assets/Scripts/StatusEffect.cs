using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour {
	internal Player player;
	internal NPC npc;
	internal float startTime;

	internal SpriteRenderer sprite;
	internal Color origColor;

	public float power = 1.0f;
	public float tickRate = 1.0f;
	public float durationInSeconds = 5.0f;

	public abstract void OnBegin ();
	public abstract void OnTick  ();
	public abstract void OnEnd   ();

	void Start() {
		player = GetComponent<Player> ();
		npc = GetComponent<NPC> ();
		StartCoroutine (_Tick ());

		if (GetComponentInChildren<SpriteRenderer> () != null) {
			sprite = GetComponentInChildren<SpriteRenderer> ();
			origColor = sprite.color;
		}
	}

	IEnumerator _Tick() {
		startTime = Time.time;
		while (gameObject.activeSelf) {
			if (durationInSeconds > 0 && Time.time - startTime >= durationInSeconds) {
				//If the duration is complete, destroy this status effect
				OnEnd ();
				Destroy (this);
				break;
			}
			//Otherwise run the regular tick method
			OnTick ();
			yield return new WaitForSeconds (tickRate);
		}
	}

	internal void SetColor(Color color) {
		if (sprite == null)
			return;

		sprite.color = color;
	}

	internal void ResetColor() {
		if (sprite == null)
			return;

		sprite.color = origColor;
	}

	internal void Spread() {

	}
}

//FIRE
public class Burning : StatusEffect {
	public override void OnBegin() {
		SetColor (Color.red);
	}

	public override void OnTick() {
		if(player != null) {
			player.TakeDamage ((int)power);
		}

		if (npc != null) {
			npc.TakeDamage ((int)power, transform.position);
		}
	}

	public override void OnEnd() {
		ResetColor ();
	}
}
	
//ICE
public class Frozen : StatusEffect {
	float origWalkSpeed;

	public override void OnBegin() {
		SetColor (Color.cyan);
	}

	public override void OnTick() {
		if (player != null) {
			player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;
		}

		if (npc != null) {
			origWalkSpeed = npc.walkSpeed;
			npc.walkSpeed = 0.0f;
		}
	}

	public override void OnEnd() {
		npc.walkSpeed = origWalkSpeed;
		ResetColor ();
	}
}

//HOLY
public class Holy : StatusEffect {
	public override void OnBegin() {

	}

	public override void OnTick() {

	}

	public override void OnEnd() {

	}
}