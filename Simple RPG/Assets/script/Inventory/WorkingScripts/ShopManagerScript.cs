using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
    //shop
    public int[,] shopItems = new int[13, 13];
    public TextMeshProUGUI CoinsTXT;
    public TextMeshProUGUI TEEtxt;
    public GameObject fullBagPanel;
    public GameObject noCoinsPanel;
    public GameObject noEnemyEquipmentPanel;
    public string StringsListCount;
    public static int newItemID;
    public static Vector2 tempLocation;

    public GameObject bagContents;

    public static int enemyEquipmentNumber;
    public static int moneyFromSale;
    public static int enemyEquipmentWorth;
    public TextMeshProUGUI enemyEquipmentNumberText;

    //bag
    public static GameObject BagObject;
    private Image imageObject;
    private GameObject Parent;

    public string BagCount;
    public int BagItemsCount;

    //scripts
    public DragDrop dragDrop;
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        enemyEquipmentWorth = 45;

        //ID's
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;
        shopItems[1, 5] = 5;
        shopItems[1, 6] = 6;
        shopItems[1, 7] = 7;
        shopItems[1, 8] = 8;
        shopItems[1, 9] = 9;

        //Price
        shopItems[2, 1] = 75;
        shopItems[2, 2] = 70;
        shopItems[2, 3] = 60;
        shopItems[2, 4] = 75;
        shopItems[2, 5] = 70;
        shopItems[2, 6] = 70;
        shopItems[2, 7] = 335;
        shopItems[2, 8] = 320;
        shopItems[2, 9] = 310;

        //Quantity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
        shopItems[3, 5] = 0;
        shopItems[3, 6] = 0;
        shopItems[3, 7] = 0;
        shopItems[3, 8] = 0;
        shopItems[3, 9] = 0;
    }

    private void Update()
    {
        CoinsTXT.text = "Coins: " + PlayerValues.coins;
        TEEtxt.text = "Equipment To Sell: " + enemyEquipmentNumber;

        enemyEquipmentNumberText.text = "You have " + enemyEquipmentNumber + " pieces of old enemy equipment";
    }

    //buying items
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (Money.spaceValue <= 11)
        {
            if (PlayerValues.coins >= shopItems[2, newItemID])
            {
                //when purchasing items
                PlayerValues.coins -= shopItems[2, newItemID];
                shopItems[3, newItemID]++;
                CoinsTXT.text = "Coins: " + PlayerValues.coins.ToString();
                ButtonRef.GetComponent<ButtonInfo>().QuantityTxt.text = shopItems[3, newItemID].ToString();

                //increase inventory space
                Money.spaceValue += 1;

                //transfering items/sprites from shop to bag
                BagObject = new GameObject("BagObject");
                imageObject = BagObject.AddComponent<Image>();
                Debug.Log(newItemID);
                GetSprite();
                ButtonInfo.BagItemsList.Add(BagObject);
                //GetBagCount();
                bagContents = GameObject.Find("BagContents");
                Parent = bagContents;
                BagObject.transform.SetParent(Parent.transform, false);
                tempLocation = BagObject.transform.position;
                BagObject.AddComponent<ShopItemUnit>();

                BagObject.AddComponent<DragDrop>();
            }

            if (PlayerValues.coins <= shopItems[2, newItemID])
            {
                noCoinsPanel.SetActive(true);
            }
        }

        if (Money.spaceValue >= 12)
        {
            if (PlayerValues.coins >= shopItems[2, newItemID])
            {
                fullBagPanel.SetActive(true);
            }
        }
    }

    public void Sell()
    {
        moneyFromSale = 45;
        PlayerValues.coins += moneyFromSale;
        enemyEquipmentNumber -= 1;
        if (enemyEquipmentNumber <= 0)
        {
            enemyEquipmentNumber = 0;
            noEnemyEquipmentPanel.SetActive(true);
        }
    }

    //getting sprite
    public void GetSprite()
    {
        switch (newItemID)
        {
            case 1:
                imageObject.sprite = ItemAssets.Instance.GreenKnightArmor;
                BagObject.tag = "Armor";
                break;
            case 2:
                imageObject.sprite = ItemAssets.Instance.GreenKnightHelmet;
                BagObject.tag = "Helmet";
                break;
            case 3:
                imageObject.sprite = ItemAssets.Instance.SandHealerStaff;
                BagObject.tag = "Weapon";
                break;
            case 4:
                imageObject.sprite = ItemAssets.Instance.ScoutArmor;
                BagObject.tag = "Armor";
                break;
            case 5:
                imageObject.sprite = ItemAssets.Instance.ScoutHelmet;
                BagObject.tag = "Helmet";
                break;
            case 6:
                imageObject.sprite = ItemAssets.Instance.DarkSword;
                BagObject.tag = "Weapon";
                break;
            case 7:
                imageObject.sprite = ItemAssets.Instance.DarkMountainArmor;
                BagObject.tag = "Armor";
                break;
            case 8:
                imageObject.sprite = ItemAssets.Instance.DarkMountainHelmet;
                BagObject.tag = "Helmet";
                break;
            case 9:
                imageObject.sprite = ItemAssets.Instance.BerserkAxe;
                BagObject.tag = "Weapon";
                break;
        }
    }
}
