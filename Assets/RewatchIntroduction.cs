using UnityEngine;
using System.Collections;

public class RewatchIntroduction : MonoBehaviour 
{
    public void Rewatch()
    {
        PlayerPrefs.DeleteKey("IntroViewed");
        Application.LoadLevel(Application.loadedLevel);
    }
}
