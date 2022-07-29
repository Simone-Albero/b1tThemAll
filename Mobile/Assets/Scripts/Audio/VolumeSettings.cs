using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider music;
    [SerializeField] Slider sfx;

    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SfxVolume";

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, music.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfx.value);
    }

    private void Awake()
    {
        music.onValueChanged.AddListener(setMusicVolume);
        sfx.onValueChanged.AddListener(setSfxVolume);
    }

    private void Start()
    {
        music.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        sfx.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
    }

    private void setMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }

    private void setSfxVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
}
