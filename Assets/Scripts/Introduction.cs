using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Introduction : MonoBehaviour
{
    public CanvasGroup introGroup;
    [Space(10)]
    public CanvasGroup[] slides;
    public GameObject[] uiToActivate;
    [Space(10)]
    public bool forceIntro;
    public bool clearPrefsOnStart;

    private int currentSlideIndex;

    private void Awake()
    {
        if (clearPrefsOnStart)
        {
            PlayerPrefs.DeleteAll();
        }

        if (!forceIntro)
        {
            //Check if the introduction already has been viewed
            if (PlayerPrefs.GetInt("IntroViewed", 0) == 0)
            {
                ShowIntro();
                PlayerPrefs.SetInt("IntroViewed", 1);
            }
            else
            {
                ActivateUI();
                gameObject.SetActive(false);
            }
        }
        else
        {
            ShowIntro();
        }
    }

    private void ActivateUI()
    {
        for (int i = 0; i < uiToActivate.Length; i++)
        {
            uiToActivate[i].SetActive(true);
        }
    }

    private void ShowIntro()
    {
        for (int i = 0; i < slides.Length; i++)
        {
            slides[i].Deactivate();
        }

        slides[0].Activate();
    }

    public void NextSlide()
    {
        slides[currentSlideIndex].Deactivate();

        if (currentSlideIndex < slides.Length - 1)
        {
            currentSlideIndex++;
            slides[currentSlideIndex].Activate();
        }
        else
        {
            slides[currentSlideIndex].Deactivate();
            introGroup.Deactivate();

            //Rule #2: Double-tab
            ActivateUI();
        }
    }
}
