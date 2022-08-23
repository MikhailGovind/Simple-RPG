using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public string actorName;
    public static int actorStrength;
    public static int actorHealth;
    public static int actorConstitution;

    public static int coins;

    private void Start()
    {
        actorHealth = 10;
        actorStrength = 7;
        actorConstitution = 15;
        coins = 90;
    }
}
