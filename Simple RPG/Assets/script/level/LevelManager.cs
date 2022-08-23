using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("TRACKING:")]
    public string currentScene;
    public int battleScore;
    [Header("ENEMY STATS:")]
    public int walkingEnemyDamage = 1;
    public int jumpingEnemyDamage = 1;
    public int shootingEnemyDamage = 1;
    [Header("CHECKPOINTS:")]
    public GameObject[] checkpointRespawnTags;

    #region NON-SERIALIZED PUBLIC FIELDS:
    [System.NonSerialized]
    public GameObject currentCheckPoint;
    [System.NonSerialized]
    public int scoreAtLastCheckPoint;
    [System.NonSerialized]
    public Transform transformAtLastCheckPoint;
    #endregion

    private GameObject _player;
    private Dictionary<int, GameObject[]> _respawns;

    void Awake()
    {
        // Get references.
        _player = GameObject.FindGameObjectWithTag("Player");
        transformAtLastCheckPoint = _player.transform;
        currentScene = SceneManager.GetActiveScene().name;

        // Update the checkpoint system.
        _respawns = new Dictionary<int, GameObject[]>();
        UpdateRespawns();

        battleScore = BattleSystem.scoreValue;
    }

    private void Start()
    {
        if (battleScore == 2)
        {
            ShopManagerScript.enemyEquipmentNumber = 2;
            GameObject.FindGameObjectWithTag("Enemy_walking").SetActive(false);
        }

        if (battleScore == 4)
        {
            ShopManagerScript.enemyEquipmentNumber = 16;
            GameObject.FindGameObjectWithTag("Enemy_walking").SetActive(false);
            GameObject.FindGameObjectWithTag("Enemy_jumping").SetActive(false);
        }

        if (battleScore >= 5)
        {
            GameObject.FindGameObjectWithTag("Enemy_walking").SetActive(false);
            GameObject.FindGameObjectWithTag("Enemy_jumping").SetActive(false);
            GameObject.FindGameObjectWithTag("Enemy_flying").SetActive(false);
        }
    }

    // Reloads the current scene.
    public void ReloadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    // Updates the checkpoit system.
    public void UpdateRespawns()
    {
        // This loop finds all the gameObjects of a specific type (coins, powerups, etc. Defined by tag)
        // and stores their positions in a dictionary so that they can be respawned when the player dies.
        for (var x = 0; x < checkpointRespawnTags.Length; x++)
        {
            string tag = checkpointRespawnTags[x].tag;
            _respawns[x] = GameObject.FindGameObjectsWithTag(tag);
        }
        //Debug.Log("1st Array of respawn's length: "  + _respawns[0].Length);
    }
}

// Stuff to do: manager now records relevant values whenever the player passes a checkpoint.
// Need to do respawning stuff, re-instantiate all the objects in the collectibles arrays.
// Consider changing the arrays from GameObject to pos transforms to save RAM

