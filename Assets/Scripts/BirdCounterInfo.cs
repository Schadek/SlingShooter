using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BirdCounterInfo : MonoBehaviour
{
    [SerializeField]
    private Text amountText;
    public Image icon;
    public Button button;
    public int Amount
    {
        get { return amount; }
        set { amount = value; UpdateAmount(); }
    }

    private int amount;

    private void UpdateAmount()
    {
        amountText.text = "" + amount;
    }
}
