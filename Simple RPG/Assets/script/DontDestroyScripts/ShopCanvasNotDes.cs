using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCanvasNotDes : MonoBehaviour
{
    public static ShopCanvasNotDes Instance;

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
