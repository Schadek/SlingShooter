using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragBird : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BirdInfo info;
    public GameObject explosion;

    private Vector3 explosionOffset = new Vector3(-0.65f, 0.65f, 0);
    private GameObject dragObject;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragObject = new GameObject("DraggedBird");
        dragObject.AddComponent<SpriteRenderer>().sprite = info.eyesOpen;
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
        DroppingBirds sling = null;

        //We could look up hovered[0] for the sling, the loop is just a safety measure in case something obstructs the sling
        foreach (GameObject i in eventData.hovered)
        {
            if (i.name == "Sling")
            {
                sling = i.GetComponent<DroppingBirds>();
            }
        }
        //if (eventData.pointerEnter && eventData.pointerEnter.CompareTag("SlingCanvas"))
        //{
        //    sling = eventData.pointerEnter.GetComponent<DroppingBirds>();
        //}

        //If no sling was found in the hovered objects, delete the dragged bird sprite and end the function
        if (!sling)
        {
            Instantiate(explosion, dragObject.transform.position + explosionOffset, Quaternion.identity);
            Destroy(dragObject);
            return;
        }

        //We add one bird of type X to the slingshot. After that we destroy the dragged sprite.
        sling.birdList.Add(info.GetComponent<Bird>().prefab);
        Destroy(dragObject);

        //Now we update the canvas attached to the sling
        sling.UpdateCanvas();
    }
}
