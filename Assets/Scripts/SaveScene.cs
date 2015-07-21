using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//Disgusting, isn't it? Saving a scene as a plain text instead of using XML or json. Sorry, I will learn to use those soon
public class SaveScene : MonoBehaviour 
{
    public CanvasGroup warningGroup;
    public CanvasGroup namingGroup;
    public string sceneName = "";
    public static SaveScene Instance { get; set; }

    private string tmpSceneName;

    private void Start()
    {
        Instance = this;
        warningGroup.Deactivate();
        namingGroup.Deactivate();

        CreateMapDirectory();
    }

    private void CreateMapDirectory()
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

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    private bool LoadedSlingShotInScene()
    {
        List<Transform> pointer = BuildingBlockMenu.Instance.sceneBlocks;

        for (int i = 0; i < pointer.Count; i++)
        {
            //Hardcoding everywhere ;_;
            if (pointer[i].name == "Sling" && pointer[i].GetComponent<DroppingBirds>().birdList.Count > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void AttemptSave()
    {
        if (GenerateErrorMessages())
        {
            return;
        }

        if (sceneName == "")
        {
            //Scene not named yet -> assume first save
            namingGroup.Activate();
        }
        else
        {
            //We assume that the player wants to overwrite the previous savefile
            Save(sceneName, BuildingBlockMenu.Instance.sceneBlocks, BuildingBlockMenu.Instance.sceneEnemies, BuildingBlockMenu.Instance.sceneBirds);
        }
    }

    private bool GenerateErrorMessages()
    {
        //First we need to validate that at least one slingshot and one enemy is in the scene. Other throw error.
        if (!LoadedSlingShotInScene())
        {
            UtilityFunctions.Instance.DisplayError("There is no loaded slingshot in the scene! Place one or load the one(s) you have with birds!");
            return true;
        }
        else if (BuildingBlockMenu.Instance.sceneEnemies.Count == 0)
        {
            UtilityFunctions.Instance.DisplayError("You placed no enemies in the scene! You must place at least one for the scene to function properly!");
            return true;
        }
        return false;
    }

    public void CheckSceneName(string newName)
    {
        if (isSafeToSave(newName))
        {
            sceneName = newName;
            Save(sceneName, BuildingBlockMenu.Instance.sceneBlocks, BuildingBlockMenu.Instance.sceneEnemies, BuildingBlockMenu.Instance.sceneBirds);
        }
        else
        {
            tmpSceneName = newName;
            warningGroup.Activate();
        }
    }

    public void ApplySceneName()
    {
        sceneName = tmpSceneName;
        Save(sceneName, BuildingBlockMenu.Instance.sceneBlocks, BuildingBlockMenu.Instance.sceneEnemies, BuildingBlockMenu.Instance.sceneBirds);
    }

    public void CloseAllSaveUI()
    {
        warningGroup.Deactivate();
        namingGroup.Deactivate();
        tmpSceneName = "";
    }

    private bool isSafeToSave(string name)
    {
        string path;
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            path = "Assets/Maps/" + name + ".txt";
        }
        else
        {
            path = Application.productName + "_Data/Maps/" + name + ".txt";
        }

        if (File.Exists(path))
        {
            return false;
        }
        return true;
    }

    private void Save(string name, List<Transform> blocks, List<Transform> enemies, List<Transform> birds)
    {
        /* Structure of file:
         * OBJECT_NAME
         * MATERIAL
         * POS_X
         * POS_Y
         * ROT_Z */
        List<string> allLines = new List<string>();

        allLines.Add("BLOCKS");
        //For easier use we add the total amount of blocks
        allLines.Add("" + blocks.Count);

        foreach (Transform i in blocks)
        {
            allLines.Add(i.name);
            allLines.Add(i.GetComponent<DragSceneBlock>().mat.ToString());
            allLines.Add(i.position.x.ToString());
            allLines.Add(i.position.y.ToString());
            allLines.Add(i.rotation.eulerAngles.z.ToString());

            //Exception for sling object
            if (i.name == "Sling")
            {
                DroppingBirds tmpDrop = i.GetComponent<DroppingBirds>();

                //To make things easier we also save the amount of birds attached to the sling (Makes it easier to read the file)
                allLines.Add("" + tmpDrop.birdList.Count);
                foreach (GameObject k in tmpDrop.birdList)
                {
                    allLines.Add(k.name);
                }
            }
        }
        
        /* Structure of file:
         * OBJECT_NAME
         * POS_X
         * POS_Y
         * ROT_Z */
        allLines.Add("ENEMIES");
        allLines.Add("" + enemies.Count);

        foreach (Transform i in enemies)
        {
            allLines.Add(i.name);
            allLines.Add(i.position.x.ToString());
            allLines.Add(i.position.y.ToString());
            allLines.Add(i.rotation.eulerAngles.z.ToString());
        }

        allLines.Add("END");

        string path;
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            path = "Assets/Maps/" + name + ".txt";
        }
        else
        {
            path = Application.productName + "_Data/Maps/" + name + ".txt";
        }

        //Finally we write all lines to the file and close the save UI
        File.WriteAllLines(path, allLines.ToArray());

        CloseAllSaveUI();
    }
}
