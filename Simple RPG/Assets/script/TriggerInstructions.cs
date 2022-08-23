using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriggerInstructions : MonoBehaviour
{
    public GameObject WalkingText;
    private Animator animator;

    private void Start()
    {
        animator = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<IntructionsManager>().anim;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetBool("isOpen", true);

        WalkingText.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        WalkingText.SetActive(false);
        animator.SetBool("isOpen", false);
    }
}
