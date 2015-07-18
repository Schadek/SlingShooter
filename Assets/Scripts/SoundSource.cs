using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour 
{
    public static SoundSource Instance;
    private AudioSource aSource;

    private void Awake()
    {
        Instance = this;
        aSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        aSource.clip = clip;
        aSource.Play();
    }
}
