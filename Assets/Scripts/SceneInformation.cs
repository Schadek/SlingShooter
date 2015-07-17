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

    private void Update()
    {
        if (enemies.Count == 0)
        {
            LoadScene.Instance.ReturnToLoadScene();
        }
    }
}
