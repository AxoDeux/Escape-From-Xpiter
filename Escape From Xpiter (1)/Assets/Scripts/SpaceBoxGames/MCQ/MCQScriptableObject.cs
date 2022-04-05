using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SpaceBoxGames/MCQ")]
public class MCQScriptableObject : ScriptableObject
{
    public string Question;
    public string Option1;
    public string Option2;
    public string Option3;
    public string Option4;
    public int rightOption;

    public string solutionReason;
}
