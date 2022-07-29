using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    [SerializeField] AudioMixer mixer;
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject); 

        DontDestroyOnLoad(gameObject);
        loadVolume();

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.output;
        }    
    }

    public void play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void decreaseVolume(string name, float value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume -= value;
    }

    public void increaseVolume(string name, float value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume += value;
    }

    private void Start()
    {
        play("Theme");
    }

    private void loadVolume()
    {
        float volume = PlayerPrefs.GetFloat(MUSIC_KEY,1f);
        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, MathF.Log10(volume)*20);

        volume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        mixer.SetFloat(VolumeSettings.MIXER_SFX, MathF.Log10(volume) * 20);

    }
}
