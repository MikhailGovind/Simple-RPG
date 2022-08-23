using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class statsManScript : MonoBehaviour
{
    public static statsManScript Instance;

    //public string unitName;
    //public int unitLevel;
    public int strength;
    public int health;
    public int constitution;

    //public int maxHealth;
    //public int currentHealth;

    //public int maxStamina;
    //public int currentStamina;

    //public int damage;
    //public int lightDamage;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        health = 10;
        strength = 7;
        constitution = 15;
    }


}
