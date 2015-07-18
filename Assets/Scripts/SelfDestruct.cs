using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour 
{
    public float time;

    private void Start()
    {
        StartCoroutine(TimeBomb());
    }

    private IEnumerator TimeBomb()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
