using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions5DD : MonoBehaviour
{
    public static Instructions5DD Instance;

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
