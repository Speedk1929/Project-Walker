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
    public List<UpgradeData> upgradeData = new List<UpgradeData>();
    public Sprite icon;
    public int allowedSelections = 1;
}
