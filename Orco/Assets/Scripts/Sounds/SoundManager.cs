﻿using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }

        s.source.Play();    
    }

    public void PlayReversed(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }

        s.source.timeSamples = s.source.clip.samples - 1;
        s.source.pitch = -1;

        s.source.Play();
    }

    public void PlayRandom(string startsWith)
    {
        Sound[] s = Array.FindAll(sounds, sound => sound.name.StartsWith(startsWith));
        if (s == null)
        {
            Debug.LogWarning("Sound " + startsWith + " not found");
            return;
        }

        int random = UnityEngine.Random.Range(0, s.Length);
        s[random].source.Play();
    }
}