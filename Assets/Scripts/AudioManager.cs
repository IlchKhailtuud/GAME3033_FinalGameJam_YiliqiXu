using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public Sound[] sounds;
    public Sound[] music;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one AudioManager in scene!");
        }
        
        instance = this;
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        
        foreach (Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        
    }

    public void Play(SoundType type,string name)
    {
        Sound s = new Sound();
        switch (type)
        {
            case SoundType.MUSIC:
            {
                s = Array.Find(music, sound => sound.name == name);
                break;
            }
            case SoundType.SFX:
            {
                s = Array.Find(sounds, sound => sound.name == name);
                break;
            }
            default: break;
        }

        if (s != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.Log($"Cannot find sound {name}");
        }
    }

    public void ChangeVolume(SoundType type, float volume)
    {
        switch (type)
        {
            case SoundType.MUSIC:
            {
                foreach (Sound s in music)
                {
                    s.source.volume = s.volume * volume;
                }
                break;
            }
            case SoundType.SFX:
            {
                foreach (Sound s in sounds)
                {
                    s.source.volume = s.volume * volume;
                }
                break;
            }
            default: break;
        }
    }

    public void PlayClickSound()
    {
        Play(SoundType.SFX,"Click");
    }
}

public enum SoundType
{
    MUSIC=0,
    SFX
}
