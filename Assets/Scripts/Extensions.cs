using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class Extensions
{
    public static void Activate(this CanvasGroup g)
    {
        g.alpha = 1f;
        g.blocksRaycasts = true;
        g.interactable = true;
    }

    public static void Deactivate(this CanvasGroup g)
    {
        g.alpha = 0f;
        g.blocksRaycasts = false;
        g.interactable = false;
    }

    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }
}
