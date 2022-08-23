using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sprite GreenKnightArmor;
    public Sprite GreenKnightHelmet;
    public Sprite SandHealerStaff;
    public Sprite ScoutArmor;
    public Sprite ScoutHelmet;
    public Sprite DarkSword;
    public Sprite DarkMountainArmor;
    public Sprite DarkMountainHelmet;
    public Sprite BerserkAxe;

    public Sprite shopSlotBackground;
}
