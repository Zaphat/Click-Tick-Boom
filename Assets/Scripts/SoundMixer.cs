using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundMixer : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public void SetMusicVolume(float volume)
    {
        if (volume <= -9)
            volume = -80;
        audioMixer.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        if (volume <= -9)
            volume = -80;
        audioMixer.SetFloat("sfxVolume", volume);
    }
}
