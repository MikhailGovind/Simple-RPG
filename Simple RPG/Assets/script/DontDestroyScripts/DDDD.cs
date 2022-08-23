using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDDD : MonoBehaviour
{
    public static DDDD Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
