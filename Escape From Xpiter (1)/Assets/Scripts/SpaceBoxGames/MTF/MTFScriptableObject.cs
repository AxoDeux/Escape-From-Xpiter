using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SpaceBoxGames/MTF")]
public class MTFScriptableObject : ScriptableObject
{
    public string textA;
    public string textB;
    public string textC;
    public string optionA;
    public string optionB;
    public string optionC;
    [Tooltip("Write Correct order wrt the texts (chests). Correct option for text A in correctOrder[0] position")]
    public int[] correctOrder = new int[3];         //Option 'int[x]' goes in Chest A, B, C respectively

    public string solutionReason;
}
