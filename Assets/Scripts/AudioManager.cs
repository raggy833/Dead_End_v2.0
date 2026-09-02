using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;
    public static AudioSource _audioSource;
    public float volume;
    public bool muted;

    private void Start()
    {
        LoadVolume();
        if (volume < 0.5f)
        {
            volume = 0.5f;
        }
        ChangeMasterVolume(volume);
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
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

    public void ChangeMasterVolume(float value)
    {
        volume = value;
        AudioListener.volume = volume;
        SaveVolume(value);
    }

    public void ToggleVolume()
    {
        foreach (Sound s in sounds)
        {
            s.source.mute = !s.source.mute;
            muted = !muted;
        }
    }

    private void SaveVolume(float value)
    {
        // PlayerPrefs.SetString("lang", instance.curLanguage);
        PlayerPrefs.SetFloat("volume", value);
    }
    private void LoadVolume()
    {
        // instance.curLanguage = PlayerPrefs.GetString("lang");
        volume = PlayerPrefs.GetFloat("volume");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " was not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " was not found!");
            return;
        }
        s.source.Stop();
    }
}
