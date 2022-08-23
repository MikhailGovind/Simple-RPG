using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//setting up different states
public enum BattleState { START, SECONDENEMY, PLAYERTURN, ENEMYTURN, WON, LOST, MAP }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject newParent;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    public Transform secondEnemyBattleStation;

    public static int scoreValue;

    public static Unit playerUnit;
    Unit enemyUnit;

    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    public ActionsHUD playerActionsHUD;

    public int heavyAttackCost;
    public int lightAttackCost;
    public int healCost;
    public int blockCost;

    private GameObject actionBoardPanel;
    private GameObject selectionPlayerCircleGameObject;
    private GameObject selectionEnemyCircleGameObject;

    public BattleState state;

    private void Awake()
    {
        heavyAttackCost = 30;
        lightAttackCost = 20;
        healCost = 20;
        blockCost = 30;
    }

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());

        //finding and hiding the actions board
        actionBoardPanel = GameObject.Find("ActionBoardPanel");
        actionBoardPanel.gameObject.SetActive(false);

        //the selection circle to show who's turn it is
        selectionPlayerCircleGameObject = GameObject.Find("SelectionCircle");
        HidePlayerSelectionCircle();

        selectionEnemyCircleGameObject = GameObject.Find("EnemySelectionCircle");
        HideEnemySelectionCircle();

        newParent = GameObject.Find("newParent");
    }

    public IEnumerator SetupBattle()
    {
        //creating characters and fixing to location
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        SpawnEnemy();

        dialogueText.text = "A " + enemyUnit.unitName + " approaches...";

        //creating HUD
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        //creating stamina text in player actions panel 
        playerActionsHUD.SetActionsHUD();

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        Playerturn();
    }

    public void SpawnEnemy()
    {
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
    }

    void Playerturn()
    {
        dialogueText.text = "Choose an action";
        actionBoardPanel.gameObject.SetActive(true);

        ShowPlayerSelectionCircle();
    }

    IEnumerator PlayerAttack()
    {
        if (playerUnit.currentStamina >= 30)
        {
            yield return new WaitForSeconds(2f);

            //Damage the enemy
            bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

            //decrease stamina
            playerUnit.currentStamina -= heavyAttackCost;
            if (playerUnit.currentStamina > playerUnit.maxStamina)
                playerUnit.currentStamina = playerUnit.maxStamina;

            //health text when attacked
            enemyHUD.SetHUD(enemyUnit);

            enemyHUD.SetHP(enemyUnit.currentHealth);
            playerHUD.SetST(playerUnit.currentStamina);
            dialogueText.text = "The attack is succesful. You have done " + playerUnit.damage + " damage.";

            yield return new WaitForSeconds(2f);

            //Check if enemy is dead
            if (isDead)
            {
                //add point to score value
                scoreValue += 1;

                Debug.Log("first" + scoreValue);

                //if enough points are reached then win game
                if (scoreValue == 5)
                {
                    Debug.Log("second" + scoreValue);

                    state = BattleState.WON;
                    EndBattle();
                }
                else if (scoreValue == 2 || scoreValue == 4)
                {
                    Debug.Log("third" + scoreValue);

                    state = BattleState.MAP;
                    EndBattle();
                }
                else if (scoreValue == 1 || scoreValue == 3)
                {
                    Debug.Log("fourth" + scoreValue);

                    //moving dead enemy to deactivated game object
                    enemyBattleStation.transform.parent = newParent.transform;

                    yield return new WaitForSeconds(0.2f);

                    //disable new parent
                    newParent.SetActive(false);

                    yield return new WaitForSeconds(0.5f);

                    //instatiating new enemy
                    GameObject EnemyGO = Instantiate(enemyPrefab, secondEnemyBattleStation);
                    enemyUnit = EnemyGO.GetComponent<Unit>();

                    enemyHUD.SetHUD(enemyUnit);

                    dialogueText.text = "A " + enemyUnit.unitName + " approaches...";

                    yield return new WaitForSeconds(1f);

                    state = BattleState.PLAYERTURN;
                    Playerturn();
                }
            }
            else
            {
                //Enemy's turn
                state = BattleState.ENEMYTURN;
                EnemyMove();
            }
        }
        else
        {
            if (playerUnit.currentStamina <= 19)
            {
                dialogueText.text = "You do not have enough stamina for an action. You faint.";

                yield return new WaitForSeconds(2f);

                state = BattleState.LOST;
                SceneManager.LoadScene("LostScene");
            }
            else
            {
                dialogueText.text = "You do not have enough stamina to use this action.";

                yield return new WaitForSeconds(2f);

                state = BattleState.PLAYERTURN;
                Playerturn();
            }
        }
    }

    IEnumerator LightPlayerAttack()
    {
        if (playerUnit.currentStamina >= 20)
        {
            yield return new WaitForSeconds(2f);

            //Damage the enemy
            bool isDead = enemyUnit.LightTakeDamage(playerUnit.lightDamage);

            //decrease stamina
            playerUnit.currentStamina -= lightAttackCost;
            if (playerUnit.currentStamina > playerUnit.maxStamina)
                playerUnit.currentStamina = playerUnit.maxStamina;

            //health text when attacked
            enemyHUD.SetHUD(enemyUnit);

            enemyHUD.SetHP(enemyUnit.currentHealth);
            playerHUD.SetST(playerUnit.currentStamina);
            dialogueText.text = "The attack is succesful. You have done " + playerUnit.lightDamage + " damage.";

            yield return new WaitForSeconds(2f);

            //Check if enemy is dead
            if (isDead)
            {
                //add point to the score value
                scoreValue += 1;

                Debug.Log("first" + scoreValue);

                //if enough points are reached then win game
                if (scoreValue == 5)
                {
                    Debug.Log("second" + scoreValue);

                    state = BattleState.WON;
                    EndBattle();
                }
                else if (scoreValue == 2 || scoreValue == 4)
                {
                    Debug.Log("third" + scoreValue);

                    state = BattleState.MAP;
                    EndBattle();
                }
                else if (scoreValue == 1 || scoreValue == 3)
                {
                    Debug.Log("fourth" + scoreValue);

                    //moving dead enemy to deactivated game object
                    enemyBattleStation.transform.parent = newParent.transform;

                    yield return new WaitForSeconds(0.2f);

                    //disable new parent
                    newParent.SetActive(false);

                    yield return new WaitForSeconds(0.5f);

                    //instatiating new enemy
                    GameObject EnemyGO = Instantiate(enemyPrefab, secondEnemyBattleStation);
                    enemyUnit = EnemyGO.GetComponent<Unit>();

                    enemyHUD.SetHUD(enemyUnit);

                    dialogueText.text = "A " + enemyUnit.unitName + " approaches...";

                    yield return new WaitForSeconds(1f);

                    state = BattleState.PLAYERTURN;
                    Playerturn();
                }
            }
            else
            {
                //Enemy's turn
                state = BattleState.ENEMYTURN;
                EnemyMove();
            }
        }
        else
        {
            if (playerUnit.currentStamina <= 19)
            {
                dialogueText.text = "You do not have enough stamina for a action. You faint.";

                yield return new WaitForSeconds(2f);

                state = BattleState.LOST;
                SceneManager.LoadScene("LostScene");
            }
            else
            {
                dialogueText.text = "You do not have enough stamina to use this action.";

                yield return new WaitForSeconds(2f);

                state = BattleState.PLAYERTURN;
                Playerturn();
            }
        }
    }

    IEnumerator PlayerHeal()
    {
        if (playerUnit.currentStamina >= 20)
        {
            if (playerUnit.currentHealth == 100)
            {
                dialogueText.text = "Your health is full.";

                yield return new WaitForSeconds(2f);

                state = BattleState.PLAYERTURN;
                Playerturn();
                GameObject.Find("ActionBoardPanel").SetActive(true);
            }
            else
            {
                playerUnit.Heal(20);

                //health text when healed
                playerHUD.SetHUD(playerUnit);

                playerHUD.SetHP(playerUnit.currentHealth);
                dialogueText.text = "You feel renewed strength!";

                yield return new WaitForSeconds(0.5f);

                //decrease stamina
                playerUnit.currentStamina -= healCost;
                if (playerUnit.currentStamina > playerUnit.maxStamina)
                    playerUnit.currentStamina = playerUnit.maxStamina;

                playerHUD.SetST(playerUnit.currentStamina);

                yield return new WaitForSeconds(2f);

                state = BattleState.ENEMYTURN;
                EnemyMove();
            }
        }
        else
        {
            if (playerUnit.currentStamina <= 19)
            {
                dialogueText.text = "You do not have enough stamina for an action. You faint.";

                yield return new WaitForSeconds(2f);

                state = BattleState.LOST;
                SceneManager.LoadScene("LostScene");
            }
            else
            {
                dialogueText.text = "You do not have enough stamina to use this action.";

                yield return new WaitForSeconds(2f);

                state = BattleState.PLAYERTURN;
                Playerturn();
            }
        }
    }

    IEnumerator PlayerBlock()
    {
        if (playerUnit.currentStamina >= 20)
        {
            yield return new WaitForSeconds(2f);

            dialogueText.text = "You try and deflect the enemies next attack.";

            //decrease stamina
            playerUnit.currentStamina -= blockCost;
            if (playerUnit.currentStamina > playerUnit.maxStamina)
                playerUnit.currentStamina = playerUnit.maxStamina;

            playerHUD.SetST(playerUnit.currentStamina);

            //yield return new WaitForSeconds(2f);

            //enemy's light attack gets nullified
            if (playerUnit.lightDamage == 61)
            {
                if (enemyUnit.currentStamina >= 20)
                {
                    yield return new WaitForSeconds(1f);

                    dialogueText.text = enemyUnit.unitName + " used light attack but it was blocked.";

                    enemyUnit.currentStamina -= lightAttackCost;
                    if (enemyUnit.currentStamina > enemyUnit.maxStamina)
                        enemyUnit.currentStamina = enemyUnit.maxStamina;

                    enemyHUD.SetST(enemyUnit.currentStamina);

                    yield return new WaitForSeconds(2f);

                    state = BattleState.PLAYERTURN;
                    Playerturn();
                }
                else
                {
                    if (enemyUnit.currentStamina <= 19)
                    {
                        dialogueText.text = "Your enemy does not have enough stamina for an action. They faint.";

                        yield return new WaitForSeconds(2f);

                        state = BattleState.WON;
                        SceneManager.LoadScene("WinScene");
                    }
                    else
                    {
                        Debug.Log("The enemy does not have enough stamina.");

                        yield return new WaitForSeconds(1f);

                        state = BattleState.PLAYERTURN;
                        Playerturn();
                    }
                }
            }
            //enemy's heavy attack gets nullified
            if (playerUnit.lightDamage == 62 || playerUnit.lightDamage == 63)
            {
                if (enemyUnit.currentStamina >= 30)
                {
                    yield return new WaitForSeconds(1f);

                    enemyUnit.currentStamina -= heavyAttackCost;
                    if (enemyUnit.currentStamina > enemyUnit.maxStamina)
                        enemyUnit.currentStamina = enemyUnit.maxStamina;

                    enemyHUD.SetST(enemyUnit.currentStamina);

                    dialogueText.text = enemyUnit.unitName + " used heavy attack but it was blocked.";

                    yield return new WaitForSeconds(2f);

                    state = BattleState.PLAYERTURN;
                    Playerturn();
                }
                else
                {
                    if (enemyUnit.currentStamina <= 19)
                    {
                        dialogueText.text = "Your enemy does not have enough stamina for an action. They faint.";

                        yield return new WaitForSeconds(2f);

                        state = BattleState.WON;
                        SceneManager.LoadScene("WinScene");
                    }
                    else
                    {
                        Debug.Log("The enemy does not have enough stamina.");

                        yield return new WaitForSeconds(1f);

                        state = BattleState.PLAYERTURN;
                        Playerturn();
                    }
                }
            }
            //enemy can still heal
            if (playerUnit.lightDamage == 64)
            {
                //enemy healing
                if (enemyUnit.currentStamina >= 30)
                {
                    if (enemyUnit.currentHealth == 100)
                    {
                        dialogueText.text = enemyUnit.unitName + "'s health was full but they consumed a health potion. They are now drunk.";

                        yield return new WaitForSeconds(2f);

                        state = BattleState.PLAYERTURN;
                        Playerturn();
                    }
                    else
                    {
                        yield return new WaitForSeconds(2f);

                        enemyUnit.Heal(10);

                        enemyHUD.SetHUD(enemyUnit);

                        //enemyUnit.SetHP(enemyUnit.currentHealth);
                        dialogueText.text = enemyUnit.unitName + " feels renewed strength!";

                        yield return new WaitForSeconds(0.5f);

                        //decrease stamina
                        enemyUnit.currentStamina -= healCost;
                        if (enemyUnit.currentStamina > enemyUnit.maxStamina)
                            enemyUnit.currentStamina = enemyUnit.maxStamina;

                        enemyHUD.SetST(enemyUnit.currentStamina);

                        yield return new WaitForSeconds(2f);

                        state = BattleState.PLAYERTURN;
                        Playerturn();
                    }
                }
                else
                {
                    if (enemyUnit.currentStamina <= 19)
                    {
                        dialogueText.text = "Your enemy does not have enough stamina for an action. They faint.";

                        yield return new WaitForSeconds(2f);

                        state = BattleState.WON;
                        SceneManager.LoadScene("WinScene");
                    }
                    else
                    {
                        Debug.Log("The enemy does not have enough stamina.");

                        yield return new WaitForSeconds(1f);

                        state = BattleState.PLAYERTURN;
                        Playerturn();
                    }
                }
            }
        }
        else
        {
            if (playerUnit.currentStamina <= 19)
            {
                dialogueText.text = "You do not have enough stamina for an action. You faint.";

                yield return new WaitForSeconds(2f);

                state = BattleState.LOST;
                SceneManager.LoadScene("LostScene");
            }
            else
            {
                dialogueText.text = "You do not have enough stamina to use this action.";

                yield return new WaitForSeconds(2f);

                state = BattleState.PLAYERTURN;
                Playerturn();
            }
        }
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        actionBoardPanel.gameObject.SetActive(false);

        StartCoroutine(PlayerAttack());
    }

    public void OnLightAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        actionBoardPanel.gameObject.SetActive(false);

        StartCoroutine(LightPlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        actionBoardPanel.gameObject.SetActive(false);

        StartCoroutine(PlayerHeal());
    }

    public void OnBlockButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        actionBoardPanel.gameObject.SetActive(false);

        StartCoroutine(PlayerBlock());
    }

    public void EnemyMove()
    {
        Debug.Log("Enemy Move");

        if (state != BattleState.ENEMYTURN)
            return;

        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        //to show text
        yield return new WaitForSeconds(2f);

        dialogueText.text = "It is " + enemyUnit.unitName + "'s turn...";

        Debug.Log("Before HidePlayerSelect");

        HidePlayerSelectionCircle();
        Debug.Log("After HidePlayerSelect");
        Debug.Log("Before ShowEnemySelect");
        ShowEnemySelectionCircle();
        Debug.Log("After ShowEnemySelect");

        //enemy heavy attack
        if (enemyUnit.currentStamina >= 20)
        {
            Debug.Log("Before heavy attack lightdamage == 61 " + playerUnit.lightDamage);
            if (playerUnit.randomizer == 1)
            {
                Debug.Log("After lightdamage == 61");
                Debug.Log(playerUnit.lightDamage);

                //light enemy attack
                yield return new WaitForSeconds(2f);

                //player takes damage and checks if player is dead
                bool isDead = playerUnit.LightTakeDamage(enemyUnit.damage);

                //decrease stamina
                enemyUnit.currentStamina -= lightAttackCost;
                if (enemyUnit.currentStamina > enemyUnit.maxStamina)
                    enemyUnit.currentStamina = enemyUnit.maxStamina;

                CodeMonkey.Utils.UtilsClass.ShakeCamera(.1f, .5f);

                playerHUD.SetHUD(playerUnit);

                playerHUD.SetHP(playerUnit.currentHealth);
                enemyHUD.SetST(enemyUnit.currentStamina);
                dialogueText.text = enemyUnit.unitName + "'s light attack is succesful. They have done " + enemyUnit.lightDamage + " damage to you.";

                yield return new WaitForSeconds(2f);

                //check if player is dead
                if (isDead)
                {
                    state = BattleState.LOST;
                    EndBattle();
                }
                else
                {
                    state = BattleState.PLAYERTURN;
                    Playerturn();
                }
            }
        }
        else
        {
            Debug.Log("Else lightdamage == 61 " + playerUnit.lightDamage);

            if (enemyUnit.currentStamina <= 19)
            {
                dialogueText.text = "Your enemy does not have enough stamina for an action. They faint";

                yield return new WaitForSeconds(2f);

                state = BattleState.WON;
                SceneManager.LoadScene("WinScene");
            }
            else
            {
                Debug.Log("The enemy does not have enough stamina.");

                yield return new WaitForSeconds(1f);

                state = BattleState.PLAYERTURN;
                Playerturn();
            }
        }

        //enemy light attack
        Debug.Log("Before enemy light attack " + enemyUnit.currentStamina + " "  + playerUnit.lightDamage);

        if (enemyUnit.currentStamina >= 30)
        {
            Debug.Log("After stam light attack " + enemyUnit.currentStamina + playerUnit.lightDamage);
            if (playerUnit.randomizer == 2)
            {
                Debug.Log("After stam + after damage enemy light attack " + enemyUnit.currentStamina + playerUnit.lightDamage);

                //heavy enemy attack
                yield return new WaitForSeconds(2f);

                //player takes damage and checks if player is dead
                bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

                //decrease stamina
                enemyUnit.currentStamina -= heavyAttackCost;
                if (enemyUnit.currentStamina > enemyUnit.maxStamina)
                    enemyUnit.currentStamina = enemyUnit.maxStamina;

                CodeMonkey.Utils.UtilsClass.ShakeCamera(.1f, .5f);

                playerHUD.SetHUD(playerUnit);

                playerHUD.SetHP(playerUnit.currentHealth);
                enemyHUD.SetST(enemyUnit.currentStamina);
                dialogueText.text = enemyUnit.unitName + "'s heavy attack is succesful. They have done " + enemyUnit.damage + " damage to you.";

                yield return new WaitForSeconds(2f);

                //check if player is dead
                if (isDead)
                {
                    state = BattleState.LOST;
                    EndBattle();
                }
                else
                {
                    state = BattleState.PLAYERTURN;
                    Playerturn();
                }
            }
        }
        else
        {
            if (enemyUnit.currentStamina <= 19)
            {
                dialogueText.text = "Your enemy does not have enough stamina for an action. They faint.";

                yield return new WaitForSeconds(2f);

                state = BattleState.WON;
                SceneManager.LoadScene("WinScene");
            }
            else
            {
                Debug.Log("The enemy does not have enough stamina.");

                yield return new WaitForSeconds(1f);

                state = BattleState.PLAYERTURN;
                Playerturn();
            }
        }

        //enemy healing
        if (enemyUnit.currentStamina >= 30)
        {
            if (playerUnit.randomizer == 3)
            {
                if (enemyUnit.currentHealth == 100)
                {
                    Debug.Log(playerUnit.lightDamage);

                    dialogueText.text = enemyUnit.unitName + "'s health was full but they consumed a health potion. They are now drunk.";

                    yield return new WaitForSeconds(2f);

                    state = BattleState.PLAYERTURN;
                    Playerturn();
                }
                else
                {
                    Debug.Log(playerUnit.lightDamage);

                    //enemy healing
                    yield return new WaitForSeconds(2f);

                    enemyUnit.Heal(20);

                    enemyHUD.SetHUD(enemyUnit);

                    //enemyUnit.SetHP(enemyUnit.currentHealth);
                    dialogueText.text = enemyUnit.unitName + " feels renewed strength!";

                    yield return new WaitForSeconds(0.5f);

                    //decrease stamina
                    enemyUnit.currentStamina -= healCost;
                    if (enemyUnit.currentStamina > enemyUnit.maxStamina)
                        enemyUnit.currentStamina = enemyUnit.maxStamina;

                    enemyHUD.SetST(enemyUnit.currentStamina);

                    yield return new WaitForSeconds(2f);

                    state = BattleState.PLAYERTURN;
                    Playerturn();
                }
            }
        }
        else
        {
            if (enemyUnit.currentStamina <= 19)
            {
                dialogueText.text = "Your enemy does not have enough stamina for an action. They faint.";

                yield return new WaitForSeconds(2f);

                state = BattleState.WON;
                SceneManager.LoadScene("WinScene");
            }
            else
            {
                Debug.Log("The enemy does not have enough stamina.");

                yield return new WaitForSeconds(1f);

                state = BattleState.PLAYERTURN;
                Playerturn();
            }
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            SceneManager.LoadScene("WinScene");
        }
        else if (state == BattleState.LOST)
        {
            SceneManager.LoadScene("LostScene");
        }
        else if (state == BattleState.MAP)
        {
            SceneManager.LoadScene("WSOA2023 - 2021 - CCF Base");
        }
    }

    //functions for the selection circle
    public void HidePlayerSelectionCircle()
    {
        selectionPlayerCircleGameObject.SetActive(false);
    }

    public void ShowPlayerSelectionCircle()
    {
        selectionPlayerCircleGameObject.SetActive(true);
    }

    public void HideEnemySelectionCircle()
    {
        selectionEnemyCircleGameObject.SetActive(false);
    }

    public void ShowEnemySelectionCircle()
    {
        selectionEnemyCircleGameObject.SetActive(true);
    }
}
