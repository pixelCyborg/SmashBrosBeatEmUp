using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour {
	internal Player player;
	internal Enemy npc;
	internal float startTime;
    internal GameObject visual;
    internal int tickIndex = 0;

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
		npc = GetComponent<Enemy> ();
		StartCoroutine (_Tick ());

		if (GetComponentInChildren<SpriteRenderer> () != null) {
			sprite = GetComponentInChildren<SpriteRenderer> ();
			origColor = sprite.color;
		}

        OnBegin();
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
            tickIndex++;
			yield return new WaitForSeconds (tickRate);
		}
	}

    public void End()
    {
        OnEnd();
        Destroy(this);
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
        visual = Instantiate(Resources.Load("Particles/FireEffect"), npc.transform) as GameObject;
        visual.transform.localPosition = Vector3.up;
	}

	public override void OnTick() {
        if (tickIndex == 0) return;

		if(player != null) {
		    Player.instance.TakeDamage((int)power, player.transform.position);
		}

		if (npc != null) {
			npc.TakeDamage ((int)power, transform.position);
		}
	}

	public override void OnEnd() {
        Destroy(visual);
		ResetColor ();
	}
}
	
//ICE
public class Frozen : StatusEffect {
	float origWalkSpeed;

	public override void OnBegin() {
        //visual = Instantiate(Resources.Load("Particles/IceEffect"), npc.transform) as GameObject;
        //visual.transform.localPosition = Vector3.up;
        SetColor(Color.cyan);
    }

	public override void OnTick() {
		if (player != null) {
			player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;
		}

		if (npc != null) {
			origWalkSpeed = npc.moveSpeed;
            npc.moveDisabled = true;
			npc.moveSpeed = 0.0f;
		}
	}

	public override void OnEnd() {
        //Destroy(visual);
        npc.moveDisabled = false;
		npc.moveSpeed = origWalkSpeed;
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