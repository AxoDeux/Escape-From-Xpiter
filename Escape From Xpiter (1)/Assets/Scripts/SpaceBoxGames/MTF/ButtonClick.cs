using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ButtonClick : MonoBehaviour, IPointerClickHandler
{
    public static event Action<Button> OptionClicked;
    public static event Action<Button> ChestClicked;


    public void OnPointerClick(PointerEventData eventData)
    {
        if(this.transform.parent.name == "Options")
        {
            Debug.Log($"{this.name} is selected");
            OptionClicked.Invoke(this.GetComponent<Button>());
            return;
        }

        if(this.transform.parent.name == "Chests")
        {
            Debug.Log($"{this.name} is selected");
            ChestClicked.Invoke(this.GetComponent<Button>());
            return;
        }

    }
}

