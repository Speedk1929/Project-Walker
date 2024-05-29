using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Upgrade 
{
    public string name;
    public string discription;
    public string upgradedVariableName;
    public double variableAmount;
    public bool wholeIncrements = false;
}
