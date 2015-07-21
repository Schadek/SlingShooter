using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DroppingBirds : MonoBehaviour
{
    public List<GameObject> birdList = new List<GameObject>();
    public GameObject birdCounterPrefab;
    public Transform birdQueue;

    private CanvasGroup birdGroup;

    private void Start()
    {
        birdGroup = GetComponentInChildren<CanvasGroup>();
    }

    public void UpdateCanvas()
    {
        //Lazy work here. Instead of checking for existing panels we simply create the whole canvas from scratch ¯\_(ツ)_/¯
        if (birdList.Count == 0)
        {
            birdGroup.Deactivate();
        }
        else
        {
            birdGroup.Activate();
            //First we destroy the objects
            for (int i = 0; i < birdGroup.transform.childCount; i++)
            {
                Destroy(birdGroup.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < birdList.Count; i++)
            {
                GameObject tmpPanel = Instantiate(birdCounterPrefab);
                tmpPanel.transform.SetParent(birdGroup.transform, false);

                BirdCounterInfo tmpInfo = tmpPanel.GetComponent<BirdCounterInfo>();
                tmpInfo.Amount = i + 1;
                tmpInfo.icon.sprite = birdList[i].GetComponent<BirdInfo>().eyesOpen;
                tmpInfo.button.onClick.AddListener(() => { birdList.RemoveAt(tmpInfo.Amount - 1); UpdateCanvas(); });
            }
        }
    }
}
