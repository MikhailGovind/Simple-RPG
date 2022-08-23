using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class StatHUD : MonoBehaviour
{
    public TextMeshProUGUI constitutionStatText;
    public TextMeshProUGUI statStatText;
    public TextMeshProUGUI healthStatText;
    public TextMeshProUGUI strenghtStatText;

    public PlayerValues playerValues;

    private void Awake()
    {
        playerValues = GameObject.Find("statsMan").GetComponent<PlayerValues>();
    }

    private void Update()
    {
        healthStatText.text = " " + PlayerValues.actorHealth;
        strenghtStatText.text = " " + PlayerValues.actorStrength;
        constitutionStatText.text = " " + PlayerValues.actorConstitution;
        statStatText.text = playerValues.actorName + "'s Stats:";
    }
}
