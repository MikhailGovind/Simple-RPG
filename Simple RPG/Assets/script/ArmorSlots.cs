using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmorSlots : MonoBehaviour, IDropHandler
{
    public bool isEquipped;
    public GameObject bagContents;
    public static Vector2 tempLocation;
    public GameObject BagObject;
    public DragDrop dragDrop;
    public GameObject wrongSlotPanel;
    public GameObject equipSlotFullPanel;
    public GameObject removeButton;

    private GameObject Parent;
    public static GameObject helmet;
    public static GameObject armor;
    public static GameObject weapon;

    #region Increase Stats
    public void IncreaseStrength(int st)
    {
        PlayerValues.actorStrength += st;
    }

    public void IncreaseHealth(int hl)
    {
        PlayerValues.actorHealth += hl;
    }

    public void IncreaseConstitution(int ct)
    {
        PlayerValues.actorConstitution += ct;
    }
    #endregion

    #region Decrease Stats
    public void DecreaseStrength(int st)
    {
        PlayerValues.actorStrength -= st;
    }

    public void DecreaseHealth(int hl)
    {
        PlayerValues.actorHealth -= hl;
    }

    public void DecreaseConstitution(int ct)
    {
        PlayerValues.actorConstitution -= ct;
    }
    #endregion

    public void removeHelmet()
    {
        bagContents = GameObject.Find("BagContents");
        Parent = bagContents;

        helmet.transform.SetParent(Parent.transform, false);
        tempLocation = helmet.transform.position;
        DecreaseHealth(helmet.GetComponent<ShopItemUnit>().healthToDec);
        DecreaseStrength(helmet.GetComponent<ShopItemUnit>().strengthToDec);
        DecreaseConstitution(helmet.GetComponent<ShopItemUnit>().constitutionToDec);
        removeButton.SetActive(false);
        helmet = null;
    }

    public void removeArmor()
    {
        bagContents = GameObject.Find("BagContents");
        Parent = bagContents;

        armor.transform.SetParent(Parent.transform, false);
        tempLocation = armor.transform.position;
        DecreaseHealth(armor.GetComponent<ShopItemUnit>().healthToDec);
        DecreaseStrength(armor.GetComponent<ShopItemUnit>().strengthToDec);
        DecreaseConstitution(armor.GetComponent<ShopItemUnit>().constitutionToDec);
        removeButton.SetActive(false);
        armor = null;
    }

    public void removeWeapon()
    {
        bagContents = GameObject.Find("BagContents");
        Parent = bagContents;

        weapon.transform.SetParent(Parent.transform, false);
        tempLocation = weapon.transform.position;
        DecreaseHealth(weapon.GetComponent<ShopItemUnit>().healthToDec);
        DecreaseStrength(weapon.GetComponent<ShopItemUnit>().strengthToDec);
        DecreaseConstitution(weapon.GetComponent<ShopItemUnit>().constitutionToDec);
        removeButton.SetActive(false);
        weapon = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        switch (gameObject.name)
        {
            case "HelmetChestSlot":
                if (eventData.pointerDrag.tag == "Helmet")
                {
                    if (helmet == null)
                    {
                        IncreaseHealth(eventData.pointerDrag.GetComponent<ShopItemUnit>().healthToInc);
                        IncreaseStrength(eventData.pointerDrag.GetComponent<ShopItemUnit>().strengthToInc);
                        IncreaseConstitution(eventData.pointerDrag.GetComponent<ShopItemUnit>().constitutionToInc);
                        removeButton.SetActive(true);
                        helmet = eventData.pointerDrag.gameObject;
                    }
                    else if (helmet != null)
                    {
                        IncreaseHealth(eventData.pointerDrag.GetComponent<ShopItemUnit>().healthToInc);
                        IncreaseStrength(eventData.pointerDrag.GetComponent<ShopItemUnit>().strengthToInc);
                        IncreaseConstitution(eventData.pointerDrag.GetComponent<ShopItemUnit>().constitutionToInc);
                        helmet = eventData.pointerDrag.gameObject;
                        equipSlotFullPanel.SetActive(true);
                        removeHelmet();
                    }
                }
                else if (eventData.pointerDrag.tag == "Armor")
                {
                    IncreaseHealth(eventData.pointerDrag.GetComponent<ShopItemUnit>().healthToInc);
                    IncreaseStrength(eventData.pointerDrag.GetComponent<ShopItemUnit>().strengthToInc);
                    IncreaseConstitution(eventData.pointerDrag.GetComponent<ShopItemUnit>().constitutionToInc);
                    armor = eventData.pointerDrag.gameObject;
                    wrongSlotPanel.SetActive(true);
                    removeArmor();
                }
                else if (eventData.pointerDrag.tag == "Weapon")
                {
                    IncreaseHealth(eventData.pointerDrag.GetComponent<ShopItemUnit>().healthToInc);
                    IncreaseStrength(eventData.pointerDrag.GetComponent<ShopItemUnit>().strengthToInc);
                    IncreaseConstitution(eventData.pointerDrag.GetComponent<ShopItemUnit>().constitutionToInc);
                    weapon = eventData.pointerDrag.gameObject;
                    wrongSlotPanel.SetActive(true);
                    removeWeapon();
                }
                break;
            case "ArmorChestSlot":
                if (eventData.pointerDrag.tag == "Armor")
                {
                    if (armor == null)
                    {
                        IncreaseHealth(eventData.pointerDrag.GetComponent<ShopItemUnit>().healthToInc);
                        IncreaseStrength(eventData.pointerDrag.GetComponent<ShopItemUnit>().strengthToInc);
                        IncreaseConstitution(eventData.pointerDrag.GetComponent<ShopItemUnit>().constitutionToInc);
                        removeButton.SetActive(true);
                        armor = eventData.pointerDrag.gameObject;
                    }
                    else if (armor != null)
                    {
                        IncreaseHealth(eventData.pointerDrag.GetComponent<ShopItemUnit>().healthToInc);
                        IncreaseStrength(eventData.pointerDrag.GetComponent<ShopItemUnit>().strengthToInc);
                        IncreaseConstitution(eventData.pointerDrag.GetComponent<ShopItemUnit>().constitutionToInc);
                        armor = eventData.pointerDrag.gameObject;
                        equipSlotFullPanel.SetActive(true);
                        removeArmor();
                    }
                }
                else if (eventData.pointerDrag.tag == "Helmet")
                {
                    IncreaseHealth(eventData.pointerDrag.GetComponent<ShopItemUnit>().healthToInc);
                    IncreaseStrength(eventData.pointerDrag.GetComponent<ShopItemUnit>().strengthToInc);
                    IncreaseConstitution(eventData.pointerDrag.GetComponent<ShopItemUnit>().constitutionToInc);
                    helmet = eventData.pointerDrag.gameObject;
                    wrongSlotPanel.SetActive(true);
                    removeHelmet();
                }
                else if (eventData.pointerDrag.tag == "Weapon")
                {
                    IncreaseHealth(eventData.pointerDrag.GetComponent<ShopItemUnit>().healthToInc);
                    IncreaseStrength(eventData.pointerDrag.GetComponent<ShopItemUnit>().strengthToInc);
                    IncreaseConstitution(eventData.pointerDrag.GetComponent<ShopItemUnit>().constitutionToInc);
                    weapon = eventData.pointerDrag.gameObject;
                    wrongSlotPanel.SetActive(true);
                    removeWeapon();
                }
                break;
            case "WeaponChestSlot":
                if (eventData.pointerDrag.tag == "Weapon")
                {
                    if (weapon == null)
                    {
                        IncreaseHealth(eventData.pointerDrag.GetComponent<ShopItemUnit>().healthToInc);
                        IncreaseStrength(eventData.pointerDrag.GetComponent<ShopItemUnit>().strengthToInc);
                        IncreaseConstitution(eventData.pointerDrag.GetComponent<ShopItemUnit>().constitutionToInc);
                        removeButton.SetActive(true);
                        weapon = eventData.pointerDrag.gameObject;
                    }
                    else if (weapon != null)
                    {
                        IncreaseHealth(eventData.pointerDrag.GetComponent<ShopItemUnit>().healthToInc);
                        IncreaseStrength(eventData.pointerDrag.GetComponent<ShopItemUnit>().strengthToInc);
                        IncreaseConstitution(eventData.pointerDrag.GetComponent<ShopItemUnit>().constitutionToInc);
                        weapon = eventData.pointerDrag.gameObject;
                        equipSlotFullPanel.SetActive(true);
                        removeWeapon();
                    }
                }
                else if (eventData.pointerDrag.tag == "Helmet")
                {
                    IncreaseHealth(eventData.pointerDrag.GetComponent<ShopItemUnit>().healthToInc);
                    IncreaseStrength(eventData.pointerDrag.GetComponent<ShopItemUnit>().strengthToInc);
                    IncreaseConstitution(eventData.pointerDrag.GetComponent<ShopItemUnit>().constitutionToInc);
                    helmet = eventData.pointerDrag.gameObject;
                    wrongSlotPanel.SetActive(true);
                    removeHelmet();
                }
                else if (eventData.pointerDrag.tag == "Armor")
                {
                    IncreaseHealth(eventData.pointerDrag.GetComponent<ShopItemUnit>().healthToInc);
                    IncreaseStrength(eventData.pointerDrag.GetComponent<ShopItemUnit>().strengthToInc);
                    IncreaseConstitution(eventData.pointerDrag.GetComponent<ShopItemUnit>().constitutionToInc);
                    armor = eventData.pointerDrag.gameObject;
                    wrongSlotPanel.SetActive(true);
                    removeArmor();
                }
                break;
        }
        Debug.Log("stats inc");
    }
}