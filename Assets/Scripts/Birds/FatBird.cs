using UnityEngine;
using System.Collections;

public class FatBird : Bird
{
    public Sprite birdHurt;
    public Sprite bloodlustBird;

    private AudioSource ASource
    {
        get
        {
            if (!aSource)
            {
                aSource = GetComponent<AudioSource>();
            }
            return aSource;
        }
    }

    private AudioSource aSource;

    public override void OnGrab()
    {
        ASource.clip = grabSound;
        ASource.Play();
    }

    public override void OnRelease()
    {
        ASource.clip = flySound;
        ASource.Play();

        GetComponent<SpriteRenderer>().sprite = bloodlustBird;
    }

    public override void OnTap()
    {
        //Do nothing
    }

    private void OnCollisionEnter2D()
    {
        GetComponent<SpriteRenderer>().sprite = birdHurt;
    }
}
