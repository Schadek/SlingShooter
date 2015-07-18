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

    public void StartWinningCondition()
    {
        StartCoroutine(WinningCondition());
    }

    private IEnumerator WinningCondition()
    {
        while (true)
        {
            if (enemies.Count == 0)
            {
                allEnemiesDead = true;
                LoadScene.Instance.clearedGroup.Activate();
                yield return StartCoroutine(AllEnemiesDead());
                break;
            }
            yield return null;
        }
    }

    private IEnumerator AllEnemiesDead()
    {
        yield return new WaitForSeconds(2f);
        LoadScene.Instance.ReturnToLoadScene();
    }
}
