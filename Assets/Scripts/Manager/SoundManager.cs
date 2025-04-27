using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoSingleton<SoundManager>
{
    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    public float masterVolumeSFX = 1f;
    public float masterVolumeBGM = 1f;
    
    [SerializeField]
    private AudioClip[] sfxAudioClips;
    private Dictionary<string, AudioClip> audioClipsDic = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        
        sfxPlayer = transform.GetChild(0).GetComponent<AudioSource>();
        bgmPlayer = transform.GetChild(1).GetComponent<AudioSource>();

        foreach (var audioClip in sfxAudioClips)
        {
            audioClipsDic.Add(audioClip.name, audioClip);
        }
    }

    public void PlaySFX(string name)
    {
        if (audioClipsDic.ContainsKey(name) == false)
        {
            
            return;
        }
        
        sfxPlayer.PlayOneShot(audioClipsDic[name]);
    }
}