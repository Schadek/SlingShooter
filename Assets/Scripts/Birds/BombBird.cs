using UnityEngine;
using System.Collections;

public class BombBird : Bird 
{
    public float radius;
    public float force;
    [Space(10)]
    public float explosionDelay;
    public LayerMask blocksAndEnemies;

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
    }
}
