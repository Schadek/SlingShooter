using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundBar : MonoBehaviour
{
    public GameObject target;
    private AudioSource musicSource;

    private void Start()
    {
        musicSource = GameObject.FindGameObjectWithTag("MusicSource").GetComponent<AudioSource>();
        target.GetComponent<Scrollbar>().onValueChanged.AddListener(
            s =>
            {
                musicSource.volume = s;
            }
            );

        target.SetActive(false);
    }

    public void UpdateScrollBarValue()
    {
        target.GetComponent<Scrollbar>().value = musicSource.volume;
    }

    public void Toggle()
    {
        if (target.activeInHierarchy)
        {
            target.SetActive(false);
        }
        else
        {
            target.SetActive(true);
        }
    }
}
