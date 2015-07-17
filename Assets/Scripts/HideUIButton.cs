using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HideUIButton : MonoBehaviour
{
    public CanvasGroup group;
    public float seconds;
    private bool hidden;

    public void Toggle()
    {
        StopAllCoroutines();
        StartCoroutine(ToggleUI());
    }

    private IEnumerator ToggleUI()
    {
        float counter = 0f;

        if (hidden)
        {
            hidden = false;
            group.interactable = true;
            group.blocksRaycasts = true;

            while (counter < 1f)
            {
                counter += Time.deltaTime / seconds;
                group.alpha = counter;
                yield return null;
            }
        }
        else
        {
            hidden = true;
            group.interactable = false;
            group.blocksRaycasts = false;

            while (counter < 1f)
            {
                counter += Time.deltaTime / seconds;
                group.alpha = 1f - counter;
                yield return null;
            }
        }
    }
}
