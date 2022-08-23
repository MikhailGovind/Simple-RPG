using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions3DD : MonoBehaviour
{
    public static Instructions3DD Instance;

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
