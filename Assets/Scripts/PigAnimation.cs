using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class PigAnimation : MonoBehaviour
{
    public float minBlink;
    public float maxBlink;
    public float minNoise;
    public float maxNoise;
    public float timeEyesClosed;
    public float magnitudeForHurt;
    public float doubleClickTime;
    [Space(10)]
    public Sprite eyesOpen;
    public Sprite eyesClosed;
    public Sprite eyesOpenHurt;
    public Sprite eyesClosedHurt;
    [Space(10)]
    public AudioClip idle;
    public AudioClip hurt;
    public AudioClip die;
    [Space(10)]
    public SpriteRenderer icon;
    public ParticleSystem pSystem;
    public AudioSource aSource;

    //[HideInInspector]
    public bool isIngame;
    [HideInInspector]
    public SceneInformation sceneInfo;

    private Rigidbody2D rBody;
    private bool isHurt;
    private bool isDoubleClickTimeWindow;

    private void Start()
    {
        UtilityFunctions.levelPurge += OnDeath;

        StartCoroutine(Blinking());
        StartCoroutine(MakingNoise());
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (!rBody)
        {
            rBody = GetComponent<Rigidbody2D>();
        }

        if (rBody.velocity.magnitude >= magnitudeForHurt && !isIngame)
        {
            Hurt();
        }
        else if (rBody.velocity.magnitude >= magnitudeForHurt)
        {
            OnDeath();
        }

        Rigidbody2D otherBody = c.gameObject.GetComponent<Rigidbody2D>();
        if (otherBody)
        {
            if (otherBody.velocity.magnitude >= magnitudeForHurt && !isIngame)
            {
                Hurt();
            }
            else if (otherBody.velocity.magnitude >= magnitudeForHurt)
            {
                OnDeath();
            }
        }
    }

    private IEnumerator Blinking()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minBlink, maxBlink));
            if (!isHurt)
            {
                icon.sprite = eyesClosed;
            }
            else
            {
                icon.sprite = eyesClosedHurt;
            }
            yield return new WaitForSeconds(timeEyesClosed);
            if (!isHurt)
            {
                icon.sprite = eyesOpen;
            }
            else
            {
                icon.sprite = eyesOpenHurt;
            }
        }
    }

    private IEnumerator MakingNoise()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minNoise, maxNoise));
            if (!aSource.isPlaying)
            {
                aSource.clip = idle;
                aSource.Play();
            }
        }
    }

    private void OnMouseDown()
    {
        if (isHurt)
        {
            if (isDoubleClickTimeWindow)
            {
                Heal();
            }
            else
            {
                //String coroutines provide easier access here
                StopCoroutine("DoubleClick");
                isDoubleClickTimeWindow = true;
                StartCoroutine("DoubleClick");
            }
        }
    }

    private IEnumerator DoubleClick()
    {
        yield return new WaitForSeconds(doubleClickTime);
        isDoubleClickTimeWindow = false;
    }

    private void Heal()
    {
        isHurt = false;
        icon.sprite = eyesOpen;
        StartCoroutine(HealCloud());
    }

    private void Hurt()
    {
        if (isIngame)
        {
            OnDeath();
        }
        else
        {
            if (isHurt)
                return;

            isHurt = true;
            icon.sprite = eyesOpenHurt;
            aSource.clip = hurt;
            aSource.Play();
        }
    }

    public void OnDeath()
    {
        aSource.clip = die;
        aSource.Play();

        if (isIngame)
        {
            sceneInfo.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
        else
        {
            Instantiate(UtilityFunctions.Instance.explosion, transform.position + UtilityFunctions.Instance.explosionOffset, Quaternion.identity);
        }

        //We need to unsubscribe
        UtilityFunctions.levelPurge -= OnDeath;
    }

    private IEnumerator HealCloud()
    {
        pSystem.Play();
        yield return null;
        pSystem.Stop();
    }
}
