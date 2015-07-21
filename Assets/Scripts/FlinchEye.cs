using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlinchEye : MonoBehaviour 
{
    public Image eye;
    public Image pupil;
    [Space(15)]
    public Sprite openedEye;
    public Sprite flinchedEye;
    [Space(15)]
    public bool flipEye;

    private void OnMouseOver()
    {
        if (flipEye)
        {
            eye.transform.rotation = Quaternion.Euler(0, 0, 180f);
        }
        eye.sprite = flinchedEye;
        pupil.enabled = false;
    }

    private void OnMouseExit()
    {
        if (flipEye)
        {
            eye.transform.rotation = Quaternion.Euler(0, 0, 180f);
        }
        eye.sprite = openedEye;
        pupil.enabled = true;
    }
}
