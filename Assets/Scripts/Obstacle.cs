using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    public float magnitudeDestruct = 1f;

    void OnCollisionEnter(Collision other)
    {
        //Ensure the colliding object is the player projectile
        if (other.gameObject.tag == "Projectile")
        {
            Rigidbody rBody = other.gameObject.GetComponent<Rigidbody>();

            Debug.Log(rBody.velocity.magnitude);
            if (rBody.velocity.magnitude >= magnitudeDestruct)
            {
                Destroy(gameObject);
            }
        }
    }
}
