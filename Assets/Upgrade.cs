using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Upgrade 
{
    public string name;
    public string description;
    public string upgradedVariableName;
    public double variableAmount;
    public bool wholeIncrements = false;
    public Sprite icon;
}
