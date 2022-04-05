using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JigsawPartRepair : MonoBehaviour
{


    [SerializeField] Text timeToRepairText;

    private void Update()
    {


        if (Move.timeToRepair <= 0f)
        {
            timeToRepairText.enabled = false;
        }
        else
        {
            timeToRepairText.text = "Time to repair :" + Move.timeToRepair.ToString("0");
        }
    }
}