using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntructionsManager : MonoBehaviour
{
    public Animator anim;

    public void StartInstructions(Intructions intructions)
    {
        anim.SetBool("isOpen", true);
    }

    void EndInstructions()
    {
        anim.SetBool("isOpen", false);
    }
}
