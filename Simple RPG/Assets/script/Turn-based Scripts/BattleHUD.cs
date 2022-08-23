using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI healthStatText;
    public TextMeshProUGUI strenghtStatText;
    public TextMeshProUGUI constitutionStatText;
    public TextMeshProUGUI statStatText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;

    public Slider hpSlider;
    public Slider stSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl: " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHealth;
        hpSlider.value = unit.currentHealth;

        stSlider.maxValue = unit.maxStamina;
        stSlider.value = unit.currentStamina;

        healthStatText.text = " " + unit.health;
        strenghtStatText.text = " " + unit.strength;
        constitutionStatText.text = " " + unit.constitution;
        statStatText.text = unit.unitName + "'s Stats:";

        healthText.text = "Health: " + unit.currentHealth + "/" + unit.maxHealth;
        staminaText.text =unit.maxStamina + " Total Stamina:";

        if (unit.currentHealth <= 0)
        {
            healthText.text = "Health: 0/" + unit.maxHealth;
        }
    }

    //hp slider and st slider
    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void SetST(int st)
    {
        stSlider.value = st;
    }
}
