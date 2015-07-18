using UnityEngine;
using System.Collections;

public class MusicSource : MonoBehaviour 
{
    public static bool musicSourceExists;

	private void Start () 
    {
        if (musicSourceExists)
        {
            Destroy(gameObject);
        }

        musicSourceExists = true;
        DontDestroyOnLoad(gameObject);
	}
}
