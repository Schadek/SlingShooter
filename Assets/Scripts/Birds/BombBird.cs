using UnityEngine;
using System.Collections;

public class BombBird : Bird 
{
    public float radius;
    public float force;
    [Space(10)]
    public float explosionDelay;
    [Space(10)]
    public AudioClip explosionSound;
    public GameObject soundPrefab;
    public GameObject explosion;
    [Space(10)]
    public LayerMask blocksAndEnemies;
    private Vector3 explosionOffset = new Vector3(-0.65f, 0.65f, 0);

    private bool triggered;

    public override void OnGrab()
    {
        //Play sound
    }

    public override void OnRelease()
    {
        //Play sound
    }

    public override void OnTap()
    {
        Explode(0.5f);
    }

    private void OnMouseDown()
    {
        rBody.isKinematic = false;
    }

    private IEnumerator OnCollisionEnter2D(Collision2D c)
    {
        if (!triggered)
        {
            triggered = true;
            yield return new WaitForSeconds(explosionDelay);
            Explode(1f);
        }
    }

    private void Explode(float forceModifier)
    {
        Collider2D[] colliderInRange = Physics2D.OverlapCircleAll(transform.position, radius, blocksAndEnemies);

        foreach (Collider2D i in colliderInRange)
        {
            i.GetComponent<Rigidbody2D>().AddExplosionForce(force * forceModifier, transform.position, radius);
        }

        //Make noise please
        AudioSource tmpAudio = Instantiate(soundPrefab).GetComponent<AudioSource>();
        tmpAudio.volume = 0.5f;
        tmpAudio.GetComponent<SoundSource>().clip = explosionSound;

        //Destroy the bird and remove it from the list
        SceneInformation.Instance.allObjects.Remove(gameObject);
        Instantiate(explosion, transform.position + explosionOffset, Quaternion.identity);
        

        //Now we must guarantee that the line renderer gets deleted at the end of the scene
        SceneInformation.Instance.allObjects.Add(transform.GetChild(0).gameObject);
        //The line renderer is parented to this object so we have to save it from destruction
        transform.GetChild(0).SetParent(null);

        Destroy(gameObject);
    }
}
