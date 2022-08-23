using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Unit : MonoBehaviour
{
    public PlayerValues playerValues;
    public int unitType;

    public string unitName;
    public int unitLevel;
    public int strength;
    public int health;
    public int constitution;

    public int maxHealth;
    public int currentHealth;

    public int maxStamina;
    public int currentStamina;

    public int damage;
    public int lightDamage;

    //randomizer
    public int randomizer;

    private void Awake()
    {
        playerValues = GameObject.Find("statsMan").GetComponent<PlayerValues>();

        if (unitType == 1)
        {
            health = PlayerValues.actorHealth;
            strength = PlayerValues.actorStrength;
            constitution = PlayerValues.actorConstitution;
        }

        #region health to maxhealth if statement

        maxHealth = health * 10;
        currentHealth = health * 10;

        #endregion

        #region constitution to maxstamina if statement

        maxStamina = constitution * 10;
        currentStamina = constitution * 10;

        #endregion
    }

    private void Update()
    {
        #region strength to heavy attack damage if statement

        damage = UnityEngine.Random.Range(strength * 10 - 4, strength * 10 + 1);

        #endregion

        #region strength to light attack damage if statement

        lightDamage = UnityEngine.Random.Range(strength * 10 - 9, strength * 10 - 5);

        #endregion

        randomizer = Random.Range(1, 4);
    }

    //heavy attack damage
    public bool TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
            return true;
        else
            return false;
    }

    //light attack damage
    public bool LightTakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
            return true;
        else
            return false;
    }

    //healing
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
}

