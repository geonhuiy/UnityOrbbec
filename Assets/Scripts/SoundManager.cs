using UnityEngine.Audio;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;

    //Initialize the sounds
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volumn;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            Debug.Log(s.source.volume);
        }
    }

    void Start()
    {
        Play("theme");
        Debug.Log("STARTED SOUND");
    }

    // Play function is public to play music in other script
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        s.source.Play();
        Debug.Log("SOUNDS " + sounds.Length);
    }
}
