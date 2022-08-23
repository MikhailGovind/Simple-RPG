using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionsHUD : MonoBehaviour
{
    public TextMeshProUGUI HACostText;
    public TextMeshProUGUI LACostText;
    public TextMeshProUGUI HCostText;
    public TextMeshProUGUI BCostText;

    public void SetActionsHUD()
    {
        HACostText.text = "30 ST";
        LACostText.text = "20 ST";
        HCostText.text = "20 ST";
        BCostText.text = "30 ST";
    }
}
