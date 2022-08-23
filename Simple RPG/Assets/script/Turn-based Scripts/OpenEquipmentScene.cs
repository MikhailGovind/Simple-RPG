using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenEquipmentScene : MonoBehaviour
{
    public void OpenEquipment()
    {
        SceneManager.LoadScene("EquipmentScene");
    }
}
