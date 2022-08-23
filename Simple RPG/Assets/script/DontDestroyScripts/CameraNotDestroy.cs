using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNotDestroy : MonoBehaviour
{
    public static CameraNotDestroy Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
