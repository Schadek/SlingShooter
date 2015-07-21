using UnityEngine;
using System.Collections;

public class KillBox : MonoBehaviour 
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.GetComponent<DragSceneBlock>().isCreature)
        {
            c.GetComponent<PigAnimation>().OnDeath();
            BuildingBlockMenu.Instance.sceneEnemies.Remove(c.transform);
            Destroy(c.gameObject);
        }
        else
        {
            BuildingBlockMenu.Instance.sceneBlocks.Remove(c.transform);
            Destroy(c.gameObject);
        }
    }
}
