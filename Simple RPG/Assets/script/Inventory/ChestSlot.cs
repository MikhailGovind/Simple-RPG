using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChestSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if ((gameObject.tag == "Chest") || (gameObject.tag == "Bag"))
            {
                eventData.pointerDrag.GetComponent<Transform>().SetParent(this.gameObject.transform, true);
                
                if (Money.spaceValue >= 1)
                {
                    Money.spaceValue -= 1;
                }
            }
        }
    }
}
