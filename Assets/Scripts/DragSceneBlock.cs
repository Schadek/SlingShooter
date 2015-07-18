using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DragSceneBlock : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    public bool isCreature;
    public Materials mat;

    private SpringJoint2D tmpJointScene;
    private Rigidbody2D tmpBodyCursor;

    public void OnBeginDrag(PointerEventData eventData)
    {
        tmpJointScene = gameObject.AddComponent<SpringJoint2D>();
        tmpBodyCursor = Instantiate(UtilityFunctions.Instance.cursorJointPrefab).GetComponent<Rigidbody2D>();

        tmpJointScene.connectedBody = tmpBodyCursor;
        tmpJointScene.distance = 0.01f;
        tmpJointScene.frequency = 0f;
    }

    private void OnMouseOver()
    {
        //Check for right click (bit of a trick because right click event does not exist)
        if (Input.GetButton("Fire2"))
        {
            if (isCreature)
            {
                BuildingBlockMenu.Instance.sceneEnemies.Remove(transform);
                GetComponent<PigAnimation>().OnDeath();
                Destroy(gameObject);
                return;
            }

            BuildingBlockMenu.Instance.sceneBlocks.Remove(transform);
            Destroy(gameObject);
            return;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(tmpBodyCursor.gameObject);
        Destroy(tmpJointScene);

        Vector3 newVelocity = Input.mousePosition - UtilityFunctions.Instance.mouseDeltaPosition;
        GetComponent<Rigidbody2D>().velocity = newVelocity * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Leave this empty. For some Unity-intern reason this must be implemented
    }
}
