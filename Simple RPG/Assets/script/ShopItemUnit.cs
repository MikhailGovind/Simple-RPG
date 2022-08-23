using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemUnit : MonoBehaviour
{
    public int healthToInc;
    public int strengthToInc;
    public int constitutionToInc;

    public int healthToDec;
    public int strengthToDec;
    public int constitutionToDec;

    private void Start()
    {
        switch (ShopManagerScript.newItemID)
        {
            case 1:
                healthToInc = 3;
                constitutionToInc = 1;
                healthToDec = 3;
                constitutionToDec = 1;
                break;
            case 2:
                healthToInc = 1;
                strengthToInc = 1;
                constitutionToInc = 3;
                healthToDec = 1;
                strengthToDec = 1;
                constitutionToDec = 3;
                break;
            case 3:
                healthToInc = 1;
                strengthToInc = 2;
                healthToDec = 1;
                strengthToDec = 2;
                break;
            case 4:
                healthToInc = 2;
                constitutionToInc = 1;
                healthToDec = 2;
                constitutionToDec = 1;
                break;
            case 5:
                strengthToInc = 1;
                constitutionToInc = 4;
                strengthToDec = 1;
                constitutionToDec = 4;
                break;
            case 6:
                healthToInc = 1;
                strengthToInc = 2;
                constitutionToInc = 2;
                healthToDec = 1;
                strengthToDec = 2;
                constitutionToDec = 2;
                break;
            case 7:
                healthToInc = 13;
                strengthToInc = 2;
                healthToDec = 13;
                strengthToDec = 2;
                break;
            case 8:
                healthToInc = 7;
                strengthToInc = 2;
                constitutionToInc = 4;
                healthToDec = 7;
                strengthToDec = 2;
                constitutionToDec = 4;
                break;
            case 9:
                healthToInc = 1;
                strengthToInc = 4;
                constitutionToInc = 3;
                healthToDec = 1;
                strengthToDec = 4;
                constitutionToDec = 3;
                break;
        }

        Debug.Log(healthToInc + " " + strengthToInc + " " + constitutionToInc);
    }
}
