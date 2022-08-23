using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions8DD : MonoBehaviour
{
    public static Instructions8DD Instance;

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
