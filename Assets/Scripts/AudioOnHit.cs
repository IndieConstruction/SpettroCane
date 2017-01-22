using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOnHit : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] clips;
    public AudioClip rareClip;

    void OnMouseUpAsButton()
    {
        source.clip = clips[Random.Range(0, clips.Length)];
        if (Random.value < 0.05f) source.clip = rareClip;
        source.Play();
    }
}
