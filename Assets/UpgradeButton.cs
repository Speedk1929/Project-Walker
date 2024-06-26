using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{

    public Button button;
    public Upgrade upgrade;

    public TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public bool selected = false;

    private void Awake()
    {
        TryGetComponent(out button);
        button.onClick.AddListener(ButtonClick);
        

    }



    public void ButtonClick()
    {

        for (int run = 0; run < upgrade.upgradeData.Count; run++)
        {

            ChangeVariable(upgrade.upgradeData[run].scriptAccessed, upgrade.upgradeData[run].upgradedVariableName, upgrade.upgradeData[run].variableAmount, upgrade.upgradeData[run].wholeIncrements);
            
        }

        selected = true;


        PlayerStats.PlayerStatsGlobal.playerLevel++;
    }

    public void OnMouseHover()
    {

        name.text = upgrade.name;
        description.text = upgrade.description;

    }


    public void ChangeVariable(object target, string variableName, double newValue, bool wholeIncrements)
    {
        // Get the type of the target object
        Type targetType = target.GetType();

        // Find the field by name
        FieldInfo fieldInfo = targetType.GetField(variableName, BindingFlags.Public | BindingFlags.Instance);

        // Check if the field was found and is not null
        if (fieldInfo != null)
        {

            if (wholeIncrements)
            {
                double fieldValue = Convert.ToDouble(fieldInfo.GetValue(target));
                fieldInfo.SetValue(target, Convert.ChangeType(fieldValue + newValue, fieldInfo.FieldType));

            }


            else
            {
                
                double fieldValue = Convert.ToDouble(fieldInfo.GetValue(target));
                fieldInfo.SetValue(target, Convert.ChangeType(fieldValue + (fieldValue * newValue), fieldInfo.FieldType));
                
            }
            // Set the new value to the field
            Debug.Log("chese");
        }
        else
        {
            Debug.LogError($"Field '{variableName}' not found in {targetType.Name}.");
        }
    }



}

