using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMDD : MonoBehaviour
{
    public static GMDD Instance;

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
