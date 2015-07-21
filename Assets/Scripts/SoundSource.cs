using UnityEngine;
using System.Collections;

public class SoundSource : MonoBehaviour 
{
    private AudioSource aSource;
    public AudioClip clip;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
        PlaySound();
    }

    public void PlaySound()
    {
        aSource.clip = clip;
        aSource.Play();
        StartCoroutine(DestroyAfterPlay());
    }

    private IEnumerator DestroyAfterPlay()
    {
        while (aSource.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
