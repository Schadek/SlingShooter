using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class DragBuildingBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BuildingBlockGUIInfo info;
    private GameObject dragObject;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragObject = Instantiate(info.prefab);
        dragObject.name = info.prefab.name;
        BuildingBlockMenu.Instance.sceneBlocks.Add(dragObject.transform);
        AssignMaterialSprite(dragObject.GetComponent<SpriteRenderer>(), dragObject.GetComponent<BuildingBlockInfo>());
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPosition;
        newPosition = mainCam.ScreenToWorldPoint(eventData.position);
        newPosition.z = 0f;
        dragObject.transform.position = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Rigidbody2D tmpObjectRigidbody = dragObject.GetComponent<Rigidbody2D>();
        AssignRigidbodyWeight(tmpObjectRigidbody);
        AssignRigidbodyVelocity(tmpObjectRigidbody);
        tmpObjectRigidbody.isKinematic = false;

        //Now we add the ability to drag the scene object around
        DragSceneBlock tmpDrag = dragObject.AddComponent<DragSceneBlock>();
        tmpDrag.mat = DetermineMaterial(tmpDrag.GetComponent<SpriteRenderer>(), tmpDrag.GetComponent<BuildingBlockInfo>());

        //In the end we delete the block information held within the object (we do not need it anymore)
        Destroy(dragObject.GetComponent<BuildingBlockInfo>());
    }

    private Materials DetermineMaterial(SpriteRenderer img, BuildingBlockInfo buildingBlockInfo)
    {
        //No switch-statement for reference types
        if (img.sprite == buildingBlockInfo.woodIcon)
        {
            return Materials.Wood;
        }
        else if (img.sprite == buildingBlockInfo.iceIcon)
        {
            return Materials.Ice;
        }
        else
        {
            return Materials.Stone;
        }
    }

    private void AssignRigidbodyWeight(Rigidbody2D rBody)
    {
        switch (BuildingBlockMenu.CurrentMaterial)
        {
            case Materials.Wood:
                rBody.mass = GlobalSettings.woodMass;
                break;
            case Materials.Ice:
                rBody.mass = GlobalSettings.iceMass;
                break;
            case Materials.Stone:
                rBody.mass = GlobalSettings.stoneMass;
                break;
        }
    }

    private void AssignRigidbodyVelocity(Rigidbody2D tmpObjectRigidbody)
    {
        Vector3 newVelocity = Input.mousePosition - UtilityFunctions.Instance.mouseDeltaPosition;
        tmpObjectRigidbody.velocity = newVelocity * 0.5f;
    }

    private void AssignMaterialSprite(SpriteRenderer img, BuildingBlockInfo info)
    {
        switch (BuildingBlockMenu.CurrentMaterial)
        {
            case Materials.Wood:
                if (info.woodIcon)
                {
                    img.sprite = info.woodIcon;
                }
                break;
            case Materials.Ice:
                if (info.iceIcon)
                {
                    img.sprite = info.iceIcon;
                }
                break;
            case Materials.Stone:
                if (info.stoneIcon)
                {
                    img.sprite = info.stoneIcon;
                }
                break;
        }

        //This acts mostly as an exception for the slingshot
        if (info.foreGroundSprite)
        {
            img.sprite = info.foreGroundSprite;
        }
    }
}
