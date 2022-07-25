using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: see about loading music and whatever too

public class SoundManager : MonoBehaviour
{
    // single instance for game
    public static SoundManager Instance { get; private set; }
    private AudioSource source;

    private void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }
}
