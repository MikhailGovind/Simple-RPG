using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Money : MonoBehaviour
{
    //this script is actually for bag space (me stupid)

    public TextMeshProUGUI spaceText;
    public static int spaceValue;

    // Start is called before the first frame update
    void Start()
    {
        spaceText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        spaceText.text = "Space: " + spaceValue + "/12";
    }
}
