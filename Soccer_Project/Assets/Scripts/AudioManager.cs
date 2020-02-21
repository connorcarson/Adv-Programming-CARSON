using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip whistle;
    private AudioSource _audioSource;
    void Start()
    {
        ServicesLocator.AudioManager = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void Whistle(){
        _audioSource.PlayOneShot(whistle);
    }
}
