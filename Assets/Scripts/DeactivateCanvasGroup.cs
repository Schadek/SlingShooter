using UnityEngine;
using System.Collections;

public class DeactivateCanvasGroup : MonoBehaviour 
{
    public CanvasGroup group;

    public void Deactivate()
    {
        group.Deactivate();
    }
}
