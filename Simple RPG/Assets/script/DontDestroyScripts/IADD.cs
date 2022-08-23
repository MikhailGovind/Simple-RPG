using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IADD : MonoBehaviour
{
    public static IADD Instance;

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
