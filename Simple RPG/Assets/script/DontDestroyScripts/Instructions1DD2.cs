using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions1DD2 : MonoBehaviour
{
    public static Instructions1DD2 Instance;

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
