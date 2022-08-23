using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonInfo : MonoBehaviour, IPointerDownHandler
{
    //shop
    public TextMeshProUGUI PriceTxt;
    public TextMeshProUGUI QuantityTxt;
    public TextMeshProUGUI NameTxt;
    public GameObject ShopManager;
    public float price;
    public string name;

    //icon
    public TextMeshProUGUI healthStatTxt;
    public TextMeshProUGUI strengthStatTxt;
    public TextMeshProUGUI constitutionStatTxt;
    public int healthStat;
    public int strenghtStat;
    public int constitutionStat;

    //bag
    public static int BagItemsNumber;
    public string StringsListCount;


    public static List<GameObject> BagItemsList = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        PriceTxt.text = "Coins: " + price.ToString();
        NameTxt.text = name.ToString();

        healthStatTxt.text = "+" + healthStat;
        strengthStatTxt.text = "+" + strenghtStat;
        constitutionStatTxt.text = "+" + constitutionStat;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (this.gameObject.name)
        {
            case "1Item":
                ShopManagerScript.newItemID = 1;
                break;
            case "2Item":
                ShopManagerScript.newItemID = 2;
                break;
            case "3Item":
                ShopManagerScript.newItemID = 3;
                break;
            case "4Item":
                ShopManagerScript.newItemID = 4;
                break;
            case "5Item":
                ShopManagerScript.newItemID = 5;
                break;
            case "6Item":
                ShopManagerScript.newItemID = 6;
                break;
            case "7Item":
                ShopManagerScript.newItemID = 7;
                break;
            case "8Item":
                ShopManagerScript.newItemID = 8;
                break;
            case "9Item":
                ShopManagerScript.newItemID = 9;
                break;
            case "10Item":
                ShopManagerScript.newItemID = 10;
                break;
            case "11Item":
                ShopManagerScript.newItemID = 11;
                break;
            case "12Item":
                ShopManagerScript.newItemID = 12;
                break;
        }

        Debug.Log(this.gameObject.name);
    }
}
