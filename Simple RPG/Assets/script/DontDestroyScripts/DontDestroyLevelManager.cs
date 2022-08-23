using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyLevelManager : MonoBehaviour
{
    public static DontDestroyLevelManager Instance;

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
