using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class DragEnemy : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        BuildingBlockMenu.Instance.sceneEnemies.Add(dragObject.transform);
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
        AssignRigidbodyWeight(tmpObjectRigidbody, tmpObjectRigidbody.GetComponent<EnemyInfo>());
        AssignRigidbodyVelocity(tmpObjectRigidbody);
        tmpObjectRigidbody.isKinematic = false;

        //Now we add the ability to drag the scene object around
        DragSceneBlock tmpDrag = dragObject.AddComponent<DragSceneBlock>();

        //Toggle isCreature flag
        tmpDrag.isCreature = true;

        //In the end we delete the block information held within the object (we do not need it anymore)
        Destroy(dragObject.GetComponent<EnemyInfo>());
    }

    private void AssignRigidbodyWeight(Rigidbody2D rBody, EnemyInfo info)
    {
        rBody.mass = info.mass;
    }

    private void AssignRigidbodyVelocity(Rigidbody2D tmpObjectRigidbody)
    {
        Vector3 newVelocity = Input.mousePosition - UtilityFunctions.Instance.mouseDeltaPosition;
        tmpObjectRigidbody.velocity = newVelocity * 0.5f;
    }
}
