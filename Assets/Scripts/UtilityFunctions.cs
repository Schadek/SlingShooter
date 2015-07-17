using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UtilityFunctions : MonoBehaviour
{
    public GameObject cursorJointPrefab;
    public GameObject explosion;
    public Vector3 explosionOffset;
    [Space(10)]
    public Text errorText;
    public CanvasGroup errorGroup;
    public CanvasGroup leaveGroup;
    [Space(10)]
    public Vector3 mouseDeltaPosition;

    public static event LevelPurge levelPurge;
    public delegate void LevelPurge();

    public static UtilityFunctions Instance { get; set; }

    private void Start()
    {
        if (!Instance)
        {
            Instance = this;
        }

        errorGroup.Deactivate();
        leaveGroup.Deactivate();
    }

    public void DisplayError(string e)
    {
        errorText.text = e;
        errorGroup.Activate();
    }

    public void CloseErrorWindow()
    {
        errorText.text = "";
        errorGroup.Deactivate();
    }

    public void ReturnToMainMenu()
    {
        Application.LoadLevel(0);
    }

    public void OpenLeaveWarning()
    {
        leaveGroup.Activate();
    }

    public void CloseLeaveWarning()
    {
        leaveGroup.Deactivate();
    }

    private void LateUpdate()
    {
        mouseDeltaPosition = Input.mousePosition;
    }

    public static Vector2 RevertMousePosValue(Vector2 mousePos)
    {
        Vector2 newMousePos;
        newMousePos.x = mousePos.x;
        newMousePos.y = -mousePos.y + Screen.height;

        return newMousePos;
    }

    public void PurgeLevel()
    {
        //First we invoke individual reactions to the purge
        if (levelPurge != null)
        {
            levelPurge();
            NullifyPurgeLevel();
        }

        //Then we delete the stage
        BuildingBlockMenu.Instance.DeleteAllSceneObjects();
    }

    public static void NullifyPurgeLevel()
    {
        levelPurge = null;
    }
}
