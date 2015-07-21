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
    public CanvasGroup noBirdsGroup;
    public CanvasGroup titleGroup;
    public CanvasGroup soundGroup;

    public GameObject[] blocks;
    public GameObject[] enemies;
    public GameObject[] birds;
    [Space(10)]
    public GameObject lineRendererPrefab;

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
        noBirdsGroup.Deactivate();
        titleGroup.Activate();
        ReturnToLoadScene();
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
        VerticalLayoutGroup tmpLayout = contentPanel.GetComponent<VerticalLayoutGroup>();

        overallHeight += tmpLayout.padding.top + tmpLayout.padding.bottom + sceneNames.Length * tmpLayout.spacing;

        contentPanel.sizeDelta = new Vector2(selectionMask.sizeDelta.x, 0);
        for (int i = 0; i < sceneNames.Length; i++)
        {
            RectTransform tmpRect = Instantiate(sceneButtonPrefab).GetComponent<RectTransform>();
            Button tmpButton = tmpRect.GetComponentInChildren<Button>();
            Text tmpText = tmpRect.GetComponentInChildren<Text>();

            //Closures are great
            int closureI = i;
            tmpButton.onClick.AddListener(() => { LoadAt(closureI); });

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
        sceneInfo.StartConditions();
        titleGroup.Deactivate();
        soundGroup.Deactivate();
    }

    private void ParseScene(string[] lines)
    {
        int currentLine = 1;
        int numberOfBlocks = int.Parse(lines[currentLine]);
        currentLine++;

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

            //Set the rigidbodies kinematic
            Rigidbody2D tmpBody = tmpObject.GetComponent<Rigidbody2D>();
            tmpBody.isKinematic = false;

            //Define Material
            switch (lines[currentLine])
            {
                case "Wood":
                    tmpObject.GetComponent<SpriteRenderer>().sprite = blocks[prefabIndex].GetComponent<BuildingBlockInfo>().woodIcon;
                    tmpBody.mass = GlobalSettings.woodMass;
                    break;
                case "Ice":
                    tmpObject.GetComponent<SpriteRenderer>().sprite = blocks[prefabIndex].GetComponent<BuildingBlockInfo>().iceIcon;
                    tmpBody.mass = GlobalSettings.iceMass;
                    break;
                case "Stone":
                    tmpObject.GetComponent<SpriteRenderer>().sprite = blocks[prefabIndex].GetComponent<BuildingBlockInfo>().stoneIcon;
                    tmpBody.mass = GlobalSettings.stoneMass;
                    break;
            }
            currentLine++;

            //Define position
            float posX = float.Parse(lines[currentLine]);
            currentLine++;
            float posY = float.Parse(lines[currentLine]);
            currentLine++;
            float rotZ = float.Parse(lines[currentLine]);
            currentLine++;

            //Apply information
            tmpObject.transform.position = new Vector3(posX, posY, 0f);
            tmpObject.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

            //Exception for sling
            if (tmpObject.name == "Sling")
            {
                tmpObject.GetComponent<SpriteRenderer>().sprite = tmpObject.GetComponent<BuildingBlockInfo>().foreGroundSprite;
                Destroy(tmpObject.GetComponent<BoxCollider2D>());
                Destroy(tmpObject.GetComponent<Rigidbody2D>());
                tmpObject.GetComponent<CircleCollider2D>().enabled = true;
                Slingshot tmpSling = tmpObject.AddComponent<Slingshot>();
                tmpSling.lineRendererPrefab = lineRendererPrefab;

                //Delete the canvas attached to the sling
                Destroy(tmpObject.GetComponentInChildren<Canvas>().gameObject);

                int steps = int.Parse(lines[currentLine]);
                currentLine++;

                //If no first fired slingshot is defined, we take the current one
                if (!sceneInfo.lastFireSlingshot)
                {
                    sceneInfo.lastFireSlingshot = tmpObject.transform;
                }

                //Look up matching bird prefabs and apply them to the sling
                for (int k = 0; k < steps; k++)
                {
                    for (int j = 0; j < birds.Length; j++)
                    {
                        if (birds[j].name == lines[currentLine])
                        {
                            sceneInfo.birds.Add(birds[j]);
                            tmpSling.birds.Add(birds[j]);
                            currentLine++;
                        }
                    }
                }
            }

            sceneInfo.allObjects.Add(tmpObject);
        }

        currentLine++;
        //Instantiate the enemies
        int numberOfEnemies = int.Parse(lines[currentLine]);

        //Reset the previously used variables 
        currentLine++;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            GameObject tmpObject = null;

            for (int k = 0; k < enemies.Length; k++)
            {
                //Define object
                if (enemies[k].name == lines[currentLine])
                {
                    tmpObject = Instantiate(enemies[k]);
                    currentLine++;
                    break;
                }
            }

            //Define position
            float posX = float.Parse(lines[currentLine]);
            currentLine++;
            float posY = float.Parse(lines[currentLine]);
            currentLine++;
            float rotZ = float.Parse(lines[currentLine]);
            currentLine++;

            //Apply information
            tmpObject.transform.position = new Vector3(posX, posY, 0f);
            tmpObject.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

            //Set the rigidbodies kinematic
            tmpObject.GetComponent<Rigidbody2D>().isKinematic = false;

            sceneInfo.allObjects.Add(tmpObject);
            sceneInfo.enemies.Add(tmpObject);
        }

        for (int i = 0; i < sceneInfo.enemies.Count; i++)
        {
            sceneInfo.enemies[i].GetComponent<PigAnimation>().isIngame = true;
            sceneInfo.enemies[i].GetComponent<PigAnimation>().sceneInfo = sceneInfo;
        }
    }

    public void ReturnToLoadScene()
    {
        ingameUI.Deactivate();
        clearedGroup.Deactivate();
        noBirdsGroup.Deactivate();

        titleGroup.Activate();
        soundGroup.Activate();
        loadGroup.Activate();

        sceneInfo.ClearScene();

        sceneInfo.StopAllCoroutines();
        sceneInfo.lastFireSlingshot = null;
    }
}
