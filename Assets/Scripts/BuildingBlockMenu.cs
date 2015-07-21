using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public enum Materials
{
    Wood,
    Ice,
    Stone
}

public enum BuildTypes
{
    Blocks,
    Enemies,
    Birds
}

public class BuildingBlockMenu : MonoBehaviour
{
    public GameObject buildBlockUIPrefab;
    public RectTransform contentPanel;
    public Image switchButtonIcon;
    public Button switchButton;
    public Image typeButton;
    public GameObject[] buildingBlocks;
    public GameObject[] enemies;
    public GameObject[] birds;
    [Space(10)]
    public Sprite woodToIce;
    public Sprite IceToStone;
    public Sprite StoneToWood;
    [Space(10)]
    public Sprite enemy;
    public Sprite block;
    public Sprite bird;
    [Space(10)]
    public Scrollbar scrollBar;
    public GameObject explosion;

    private BuildingBlockGUIInfo[] guiInfos;

    private BuildingBlockInfo[] blockInfo;
    private EnemyInfo[] enemyInfo;
    private BirdInfo[] birdInfo;

    [HideInInspector]
    public List<Transform> sceneBlocks = new List<Transform>();
    [HideInInspector]
    public List<Transform> sceneEnemies = new List<Transform>();
    [HideInInspector]
    public List<Transform> sceneBirds = new List<Transform>();

    private static Materials currentMaterial;
    public static Materials CurrentMaterial { get { return currentMaterial; } }
    private static BuildTypes currentType;
    public static BuildTypes CurrentType { get { return currentType; } }

    public static BuildingBlockMenu Instance { get; set; }

    private void Start()
    {
        Instance = this;

        //Event here
        guiInfos = new BuildingBlockGUIInfo[buildingBlocks.Length];

        blockInfo = new BuildingBlockInfo[buildingBlocks.Length];
        enemyInfo = new EnemyInfo[enemies.Length];
        birdInfo = new BirdInfo[birds.Length];

        currentType = BuildTypes.Blocks;

        ShowBuildingBlocksUI();
        ChangeMaterial(Materials.Wood);
    }

    public void DeleteAllSceneObjects()
    {
        SaveScene.Instance.sceneName = "";
        StartCoroutine(DelayedDestruction());
    }

    private IEnumerator DelayedDestruction()
    {
        yield return null;
        for (int i = 0; i < sceneBlocks.Count; i++)
        {
            Destroy(sceneBlocks[i].gameObject);
        }
        sceneBlocks.Clear();

        for (int i = 0; i < sceneEnemies.Count; i++)
        {
            Destroy(sceneEnemies[i].gameObject);
        }
        sceneEnemies.Clear();

        for (int i = 0; i < sceneBirds.Count; i++)
        {
            Destroy(sceneBirds[i].gameObject);
        }
        sceneBirds.Clear();
    }

    private void ClearBuildingList()
    {
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            Destroy(contentPanel.GetChild(i).gameObject);
        }
    }

    public void SwitchBuildType()
    {
        ClearBuildingList();
        scrollBar.value = 1f;

        switch (currentType)
        {
            case BuildTypes.Blocks:
                ShowEnemiesUI();
                typeButton.sprite = bird;
                switchButton.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
                switchButtonIcon.color = Color.grey;
                switchButtonIcon.sprite = woodToIce;
                currentType = BuildTypes.Enemies;
                break;
            case BuildTypes.Enemies:
                ShowBirdsUI();
                typeButton.sprite = block;
                switchButtonIcon.color = Color.grey;
                switchButtonIcon.sprite = woodToIce;
                currentType = BuildTypes.Birds;
                break;
            case BuildTypes.Birds:
                ShowBuildingBlocksUI();
                ChangeMaterial(Materials.Wood);
                typeButton.sprite = enemy;
                switchButton.onClick.SetPersistentListenerState(0, UnityEventCallState.RuntimeOnly);
                switchButtonIcon.color = Color.white;
                currentType = BuildTypes.Blocks;
                break;
        }
    }

    private void ShowBuildingBlocksUI()
    {
        float overallHeight = 0f;

        contentPanel.sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, 0);
        for (int i = 0; i < buildingBlocks.Length; i++)
        {
            RectTransform tmpRect = Instantiate(buildBlockUIPrefab).GetComponent<RectTransform>();
            BuildingBlockGUIInfo tmpGUIInfo = tmpRect.GetComponent<BuildingBlockGUIInfo>();

            blockInfo[i] = buildingBlocks[i].GetComponent<BuildingBlockInfo>();
            guiInfos[i] = tmpGUIInfo;

            tmpGUIInfo.icon.sprite = blockInfo[i].woodIcon;
            //tmpGUIInfo.title.text = buildingBlocks[i].name;
            tmpGUIInfo.prefab = buildingBlocks[i];
            tmpRect.SetParent(contentPanel);
            overallHeight += tmpRect.sizeDelta.y;

            tmpGUIInfo.icon.SetNativeSize();
            //tmpGUIInfo.icon.rectTransform.

            //We need to distinguish between enemy objects and blocks. Therefore we give each button the respective drag behaviour
            DragBuildingBlock tmpDrag = tmpRect.gameObject.AddComponent<DragBuildingBlock>();
            tmpDrag.info = tmpRect.GetComponent<BuildingBlockGUIInfo>();
        }
        currentMaterial = Materials.Wood;
        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, overallHeight);
    }

    private void ShowEnemiesUI()
    {
        float overallHeight = 0f;

        contentPanel.sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, 0);
        for (int i = 0; i < enemies.Length; i++)
        {
            RectTransform tmpRect = Instantiate(buildBlockUIPrefab).GetComponent<RectTransform>();
            BuildingBlockGUIInfo tmpGUIInfo = tmpRect.GetComponent<BuildingBlockGUIInfo>();

            enemyInfo[i] = enemies[i].GetComponent<EnemyInfo>();
            guiInfos[i] = tmpGUIInfo;

            tmpGUIInfo.icon.sprite = enemyInfo[i].icon;
            tmpGUIInfo.prefab = enemies[i];
            tmpRect.SetParent(contentPanel);
            overallHeight += tmpRect.sizeDelta.y;

            tmpGUIInfo.icon.SetNativeSize();

            //We need to distinguish between enemy objects and blocks. Therefore we give each button the respective drag behaviour
            DragEnemy tmpDrag = tmpRect.gameObject.AddComponent<DragEnemy>();
            tmpDrag.info = tmpRect.GetComponent<BuildingBlockGUIInfo>();
        }
        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, overallHeight);
    }

    private void ShowBirdsUI()
    {
        float overallHeight = 0f;

        contentPanel.sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, 0);
        for (int i = 0; i < birds.Length; i++)
        {
            RectTransform tmpRect = Instantiate(buildBlockUIPrefab).GetComponent<RectTransform>();
            BuildingBlockGUIInfo tmpGUIInfo = tmpRect.GetComponent<BuildingBlockGUIInfo>();

            birdInfo[i] = birds[i].GetComponent<BirdInfo>();
            guiInfos[i] = tmpGUIInfo;

            tmpGUIInfo.icon.sprite = birdInfo[i].eyesOpen;
            tmpGUIInfo.prefab = birds[i];
            tmpRect.SetParent(contentPanel);
            overallHeight += tmpRect.sizeDelta.y;

            tmpGUIInfo.icon.SetNativeSize();

            //We need to distinguish between birds, blocks and enemies. Therefore we need to add different behaviours for each type.
            DragBird tmpDrag = tmpRect.gameObject.AddComponent<DragBird>();
            tmpDrag.info = birds[i].GetComponent<BirdInfo>();
            tmpDrag.explosion = explosion;
        }
        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, overallHeight);
    }

    private void DestroyBuildingBlocksUI()
    {
        //Just destroy your children! DO IT!
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            Destroy(contentPanel.GetChild(i).gameObject);
        }
    }

    public void NextMaterial()
    {
        switch (currentMaterial)
        {
            case Materials.Wood:
                ChangeMaterial(Materials.Ice);
                break;
            case Materials.Ice:
                ChangeMaterial(Materials.Stone);
                break;
            case Materials.Stone:
                ChangeMaterial(Materials.Wood);
                break;
        }
    }

    private void ChangeMaterial(Materials m)
    {
        for (int i = 0; i < guiInfos.Length; i++)
        {
            switch (m)
            {
                case Materials.Wood:
                    if (blockInfo[i].woodIcon)
                    {
                        guiInfos[i].icon.sprite = blockInfo[i].woodIcon;
                        switchButtonIcon.sprite = woodToIce;
                    }
                    break;
                case Materials.Ice:
                    if (blockInfo[i].iceIcon)
                    {
                        guiInfos[i].icon.sprite = blockInfo[i].iceIcon;
                        switchButtonIcon.sprite = IceToStone;
                    }
                    break;
                case Materials.Stone:
                    if (blockInfo[i].stoneIcon)
                    {
                        guiInfos[i].icon.sprite = blockInfo[i].stoneIcon;
                        switchButtonIcon.sprite = StoneToWood;
                    }
                    break;
            }
            TurnVisible(guiInfos[i].icon, blockInfo[i]);
        }

        currentMaterial = m;
    }

    private void TurnVisible(Image img, BuildingBlockInfo info)
    {
        if (img.sprite)
            return;

        //Hard-coded because there are only three materials. Oooh bad coding practice ~(°O°~)
        img.sprite = info.woodIcon;
        img.SetNativeSize();
        if (img.sprite)
            return;

        img.sprite = info.iceIcon;
        img.SetNativeSize();
        if (img.sprite)
            return;

        img.sprite = info.stoneIcon;
        img.SetNativeSize();
        if (img.sprite)
            return;
    }
}
