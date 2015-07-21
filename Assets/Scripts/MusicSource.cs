using UnityEngine;
using System.Collections;

public class MusicSource : MonoBehaviour 
{
    public static bool musicSourceExists;
    private AudioSource aSource;

	private void Start () 
    {
        if (musicSourceExists)
        {
            Destroy(gameObject);
        }

        musicSourceExists = true;
        DontDestroyOnLoad(gameObject);

        aSource = GetComponent<AudioSource>();
	}

    public void ToggleSound()
    {
        aSource.mute = !aSource.mute;
    }
}
