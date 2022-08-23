using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region CLASS DESCRIPTION:
/* 
 * This class can be attached to a trigger volume to turn it into a checkpoint.
 * It tells the level manager to store information about the level and the player
 * when the checkpoint is reached so that it can be re-instantiated if the player dies. 
 * 
 * NOTES: The checkpoint should be on the Trigger Volumes layer.
*/
#endregion

public class CheckPoint : MonoBehaviour
{
    private GameObject _player;
    private LevelManager _lvlMngr;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _lvlMngr = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == _player)
        {
            _lvlMngr.transformAtLastCheckPoint = this.transform;
            _lvlMngr.UpdateRespawns();
            
            //Debug.Log(_lvlMngr.transformAtLastCheckPoint.position);
            //Debug.Log(_lvlMngr.scoreAtLastCheckPoint);

            Destroy(gameObject);  // Respawn when next checkpoint is reached.
        }
    }
}
