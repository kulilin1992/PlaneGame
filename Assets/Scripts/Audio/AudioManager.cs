using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistenSingleton<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;

    const float MIN_PITCH = 0.9f;
    const float MAX_PITCH = 1.1f;

    public void PlaySFX(AudioData audioData)
    {
        //sFXPlayer.clip = audioClip;
        //sFXPlayer.volume = volume;
        //sFXPlayer.Play();
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    // public void PlayRandomSFX(AudioClip audioClips, float volume)
    // {
    //     sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
    //     PlaySFX(audioClips, volume);
    // }

    public void PlayRandomSFX(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
        PlaySFX(audioData);
    }

    public void PlayRandomSFX(AudioData[] audioDatas)
    {
        PlayRandomSFX(audioDatas[Random.Range(0, audioDatas.Length)]);
    }
}
[System.Serializable] public class AudioData
{
    public AudioClip audioClip;
    public float volume;
}
