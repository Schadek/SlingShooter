using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class LoadScene : MonoBehaviour
{
    public RectTransform selectionMask;
    public RectTransform contentPanel;
    public GameObject sceneButtonPrefab;
    public SceneInformation sceneInfo;
    [Space(10)]
    public CanvasGroup loadGroup;
    public CanvasGroup ingameUI;
    public CanvasGroup clearedGroup;
    public CanvasGroup titleGroup;

    public GameObject[] blocks;
    public GameObject[] enemies;
    public GameObject[] birds;
    [Space(10)]
    public GameObject projectilePrefab;

    private string[] sceneNames;
    public static LoadScene Instance;

    private void Start()
    {
        Instance = this;

        ReadSceneNames();
        ShowLevelSelection();

        //Deactivate the ingame UI. We activate it again later.
        ingameUI.Deactivate();
        clearedGroup.Deactivate();
        titleGroup.Activate();
    }

    private void ReadSceneNames()
    {
        string path;
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            path = "Assets/Maps";
        }
        else
        {
            path = Application.productName + "_Data/Maps";
        }

        sceneNames = Directory.GetFiles(path, "*.txt");
    }

    private void ShowLevelSelection()
    {
        float overallHeight = 0f;

        contentPanel.sizeDelta = new Vector2(selectionMask.sizeDelta.x, 0);
        for (int i = 0; i < sceneNames.Length; i++)
        {
            RectTransform tmpRect = Instantiate(sceneButtonPrefab).GetComponent<RectTransform>();
            Button tmpButton = tmpRect.GetComponentInChildren<Button>();
            Text tmpText = tmpRect.GetComponentInChildren<Text>();

            int heapI = i;
            tmpButton.onClick.AddListener(() => { LoadAt(heapI); });

            //Because GetFiles returns complete paths relative to the invoker we have to give the text component a cleaned up version
            string cleanedUpName = sceneNames[i];
            cleanedUpName = cleanedUpName.Substring(cleanedUpName.LastIndexOf("\\") + 1);
            cleanedUpName = cleanedUpName.Remove(cleanedUpName.Length - 4);

            tmpText.text = cleanedUpName;
            tmpRect.SetParent(contentPanel);
            overallHeight += tmpRect.sizeDelta.y;
        }
        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, overallHeight);
    }

    private void LoadAt(int i)
    {
        string[] tmpLines = File.ReadAllLines(sceneNames[i]);
        ParseScene(tmpLines);
        loadGroup.Deactivate();
        ingameUI.Activate();

        //We start the coroutine to check for a win 
        sceneInfo.StartWinningCondition();
        titleGroup.Deactivate();
    }

    private void ParseScene(string[] lines)
    {
        int numberOfBlocks = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "ENEMIES")
            {
                numberOfBlocks = (i - 1) / 5;
            }
        }

        int currentLine = 1;
        for (int i = 0; i < numberOfBlocks; i++)
        {
            GameObject tmpObject = null;
            int prefabIndex = 0;

            for (int k = 0; k < blocks.Length; k++)
            {
                //Define object
                if (blocks[k].name == lines[currentLine])
                {
                    tmpObject = Instantiate(blocks[k]);

                    tmpObject.name = lines[currentLine];
                    prefabIndex = k;
                    currentLine++;
                    break;
                }
            }

            //Define Material
            switch (lines[currentLine])
            {
                case "Wood":
                    tmpObject.GetComponent<SpriteRenderer>().sprite = blocks[prefabIndex].GetComponent<BuildingBlockInfo>().woodIcon;
                    break;
                case "Ice":
                    tmpObject.GetComponent<SpriteRenderer>().sprite = blocks[prefabIndex].GetComponent<BuildingBlockInfo>().iceIcon;
                    break;
                case "Stone":
                    tmpObject.GetComponent<SpriteRenderer>().sprite = blocks[prefabIndex].GetComponent<BuildingBlockInfo>().stoneIcon;
                    break;
            }
            currentLine++;

            //Define position
            float posX;
            float.TryParse(lines[currentLine], out posX);
            currentLine++;
            float posY;
            float.TryParse(lines[currentLine], out posY);
            currentLine++;
            float rotZ;
            float.TryParse(lines[currentLine], out rotZ);
            currentLine++;

            //Apply information
            tmpObject.transform.position = new Vector3(posX, posY, 0f);
            tmpObject.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

            //Set the rigidbodies kinematic
            tmpObject.GetComponent<Rigidbody2D>().isKinematic = false;

            sceneInfo.allObjects.Add(tmpObject);
        }

        //Instantiate the enemies
        int numberOfEnemies = 0;

        for (int i = numberOfBlocks * 5; i < lines.Length; i++)
        {
            if (lines[i] == "BIRDS")
            {
                numberOfEnemies = ((i - 2) - numberOfBlocks * 5) / 4;
            }
        }

        //Reset the previously used variables 
        //Currently we are at the ENEMIES milestone -> one line further
        currentLine++;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            GameObject tmpObject = null;
            int prefabIndex = 0;

            for (int k = 0; k < enemies.Length; k++)
            {
                //Define object
                if (enemies[k].name == lines[currentLine])
                {
                    tmpObject = Instantiate(enemies[k]);
                    prefabIndex = k;
                    currentLine++;
                    break;
                }
            }

            //Define position
            float posX;
            float.TryParse(lines[currentLine], out posX);
            currentLine++;
            float posY;
            float.TryParse(lines[currentLine], out posY);
            currentLine++;
            float rotZ;
            float.TryParse(lines[currentLine], out rotZ);
            currentLine++;

            //Apply information
            tmpObject.transform.position = new Vector3(posX, posY, 0f);
            tmpObject.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

            //Set the rigidbodies kinematic
            tmpObject.GetComponent<Rigidbody2D>().isKinematic = false;

            sceneInfo.allObjects.Add(tmpObject);
            sceneInfo.enemies.Add(tmpObject);
        }

        //BIRDS ARE NOT SUPPORTED IN THIS VERSION. SORRY MARTIN. 


        //We search for each sling and apply special rules to them. We could have avoided the search by applying these
        //rules directly as we instantiate them but there was a bug and bla

        for (int i = 0; i < sceneInfo.allObjects.Count; i++)
        {
            if (sceneInfo.allObjects[i].name == "Sling")
            {
                sceneInfo.allObjects[i].GetComponent<SpriteRenderer>().sprite = sceneInfo.allObjects[i].GetComponent<BuildingBlockInfo>().foreGroundSprite;
                Destroy(sceneInfo.allObjects[i].GetComponent<BoxCollider2D>());
                Destroy(sceneInfo.allObjects[i].GetComponent<Rigidbody2D>());
                sceneInfo.allObjects[i].GetComponent<CircleCollider2D>().enabled = true;
                Slingshot tmpSling = sceneInfo.allObjects[i].AddComponent<Slingshot>();
                tmpSling.prefabProjectile = projectilePrefab;

            }
        }

        for (int i = 0; i < sceneInfo.enemies.Count; i++)
        {
            sceneInfo.enemies[i].GetComponent<PigAnimation>().isIngame = true;
            sceneInfo.enemies[i].GetComponent<PigAnimation>().sceneInfo = sceneInfo;
        }
    }

    public void ReturnToLoadScene()
    {
        loadGroup.Activate();
        ingameUI.Deactivate();
        clearedGroup.Deactivate();
        titleGroup.Activate();

        sceneInfo.ClearScene();

        sceneInfo.StopAllCoroutines();
    }
}
