using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioHandler {
    internal AudioSource source;

    public AudioHandler() { }

    public AudioHandler(AudioSource _source)
    {
        Initialize(_source);
    }

    public void Initialize(AudioSource _source)
    {
        source = _source;
    }

    internal void PlaySound(AudioClip[] clips, bool layered = false)
    {
        if(clips == null || clips.Length < 1)
        {
            Debug.Log("No audio clips provided");
            return;
        }
        PlaySound(clips[Random.Range(0, clips.Length)], layered);
    }

    internal void PlaySound(AudioClip clip, bool layered = false)
    {
        if(source == null)
        {
            Debug.Log("Audio source not initialized");
            return;
        }

        if (clip == null)
        {
            Debug.Log("No audio clips provided");
            return;
        }

        if (layered)
        {
            source.PlayOneShot(clip, source.volume * 0.5f);
        }
        else
        {
            source.clip = clip;
            source.Play();
        }
    }
}

[System.Serializable]
public class CharacterAudioHandler : AudioHandler
{
    private Vector2 prevPos;

    public float footstepsDistance = 2.0f;
    public AudioClip[] footsteps;
    public AudioClip[] jump;
    public AudioClip[] impact;
    public AudioClip[] takeDamage;
    public AudioClip defaultItemPickup;
    public AudioClip die;

    public void PlayFootstep(Vector2 position)
    {
        if (prevPos == Vector2.zero || Vector2.Distance(prevPos, position) > footstepsDistance)
        {
            PlayFootstep();
            prevPos = position;
        }
    }

    public void PlayFootstep()
    {
        PlaySound(footsteps);
    }

    public void PlayJump()
    {
        PlaySound(jump);
    }

    public void PlayImpact()
    {
        PlaySound(impact);
    }

    public void PlayDamage()
    {
        PlaySound(takeDamage);
    }

    public void PlayDeath()
    {
        PlaySound(die);
    }

    public void PlayPickup()
    {
        PlaySound(defaultItemPickup);
    }
}

[System.Serializable]
public class WeaponAudioHandler : AudioHandler
{
    public AudioClip[] shoot;
    public AudioClip reload;

    public void PlayShoot()
    {
        PlaySound(shoot);
    }
}

[System.Serializable]
public class UiAudioHandler : AudioHandler
{
    public AudioClip showScreen;
    public AudioClip hideScreen;

    public AudioClip hoverInventory;
    public AudioClip selectInventory;

    public AudioClip hoverButton;
    public AudioClip clickButton;

    public AudioClip hoverMap;
    public AudioClip selectMap;

    public void PlayShowScreen()
    {
        PlaySound(showScreen);
    }

    public void PlayHideScreen()
    {
        PlaySound(hideScreen);
    }

    public void PlayInventoryHover()
    {
        PlaySound(hoverInventory);
    }

    public void PlayInventoryUse()
    {
        PlaySound(selectInventory);
    }

    public void PlayMapHover()
    {
        PlaySound(hoverMap);
    }

    public void PlayMapSelect()
    {
        PlaySound(selectMap);
    }
}

[System.Serializable]
public class DialogAudioHandler : AudioHandler
{
    public AudioClip closeDialog;
    public AudioClip[] dialogBlip;

    public void PlayCloseDialog()
    {
        PlaySound(closeDialog);
    }

    public void PlayDialog()
    {
        PlaySound(dialogBlip);
    }
}

[System.Serializable]
public class InteractionAudioHandler : AudioHandler
{
    public AudioClip interactionSelect;
    public AudioClip interactionDeselect;

    public void PlaySelect()
    {
        PlaySound(interactionSelect);
    }

    public void PlayDeselect()
    {
        PlaySound(interactionDeselect);
    }
}