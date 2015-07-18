using UnityEngine;
using System.Collections;

public class KillBox : MonoBehaviour 
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        Debug.Log("Obj destroyed");
        if (c.GetComponent<DragSceneBlock>().isCreature)
        {
            Debug.Log("Was Creature");
            c.GetComponent<PigAnimation>().OnDeath();
            BuildingBlockMenu.Instance.sceneEnemies.Remove(c.transform);
            Destroy(c.gameObject);
        }
        else
        {
            Debug.Log("Was Block");
            BuildingBlockMenu.Instance.sceneBlocks.Remove(c.transform);
            Destroy(c.gameObject);
        }
    }
}
