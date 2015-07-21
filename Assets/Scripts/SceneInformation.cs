using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SceneInformation : MonoBehaviour
{
    public static SceneInformation Instance;

    public List<GameObject> allObjects;
    public List<GameObject> enemies;
    public List<GameObject> birds;
    public List<GameObject> projectiles;
    public Rigidbody2D currentProjectile;
    public LineRenderer currentLineRenderer;
    public Transform lastFireSlingshot;

    public static bool allEnemiesDead;

    private void Awake()
    {
        Instance = this;
    }

    public void ClearScene()
    {
        for (int i = 0; i < allObjects.Count; i++)
        {
            Destroy(allObjects[i]);
        }
        allObjects.Clear();
        enemies.Clear();
        birds.Clear();
        projectiles.Clear();

        UtilityFunctions.NullifyPurgeLevel();
    }

    public void StartConditions()
    {
        StartCoroutine(WinningCondition());
        StartCoroutine(LosingCondition());
    }

    private IEnumerator WinningCondition()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (enemies.Count == 0)
            {
                allEnemiesDead = true;
                LoadScene.Instance.clearedGroup.Activate();
                yield return StartCoroutine(AllEnemiesDead());
                break;
            }
        }
    }

    private IEnumerator LosingCondition()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (birds.Count == 0 && AllProjectilesSleeping() && AllObjectsSleeping())
            {
                LoadScene.Instance.noBirdsGroup.Activate();
                yield return StartCoroutine(AllEnemiesDead());
                break;
            }
        }
    }

    private bool AllObjectsSleeping()
    {
        foreach (GameObject i in allObjects)
        {
            Rigidbody2D rBody = i.GetComponent<Rigidbody2D>();
            if (rBody != null && !rBody.IsSleeping())
            {
                return false;
            }
        }
        return true;
    }

    private bool AllProjectilesSleeping()
    {
        /*foreach (GameObject i in projectiles)
        {
            if (!i.GetComponent<Rigidbody2D>().IsSleeping())
            {
                return false;
            }
        }
        return true;*/
        if (currentProjectile)
            return currentProjectile.IsSleeping();
        return true;
    }

    private IEnumerator AllEnemiesDead()
    {
        yield return new WaitForSeconds(2f);
        LoadScene.Instance.ReturnToLoadScene();
    }
}
