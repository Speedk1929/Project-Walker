using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeData 
{
    public string upgradedVariableName;
    public double variableAmount;
    public bool wholeIncrements = false;
    public MonoBehaviour scriptAccessed;

    public double minimum;
    public double max;
}
