using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions7DD : MonoBehaviour
{
    public static Instructions7DD Instance;

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
