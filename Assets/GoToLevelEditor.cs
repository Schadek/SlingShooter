using UnityEngine;
using System.Collections;

public class GoToLevelEditor : MonoBehaviour 
{
    public void LoadScene(int index)
    {
        Application.LoadLevel(index);
    }
}
