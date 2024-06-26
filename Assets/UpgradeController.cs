using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{

    public List<Upgrade> upgrades = new List<Upgrade>();
    public List<Upgrade> upgradesSelected = new List<Upgrade>();

    public int maxAvailableUpgrades = 4;
    public List<Upgrade> upgradesAvailable = new List<Upgrade>();
    public GameObject upgradeButtonPrefab;
    public GameObject upgradePanel;
    public GameObject upgradeDescriptions;

    public TextMeshProUGUI name;
    public TextMeshProUGUI description;



    private void Start()
    {

        maxAvailableUpgrades = Convert.ToInt32(MathF.Min(maxAvailableUpgrades, upgrades.Count));
        PlayerStats.PlayerStatsGlobal.upgrade += LevelUp;
        upgradePanel.GetComponent<Image>().enabled = false;
        upgradeDescriptions.SetActive(false);
    }


    public void LevelUp()
    {

        StartCoroutine(LevelUpCall());
        

    }

    public List<Upgrade> CheckAvailableUpgrades()
    {
        List<Upgrade> upgradesChecked = new List<Upgrade>();


        foreach (Upgrade upgrade in upgrades)
        {

            if (upgrade.allowedSelections > 0)
            {

                int satisfiedConiditions = 0;
                

                foreach (UpgradeData upgradeData in upgrade.upgradeData)
                {

                    if (CheckLimitations(upgradeData.scriptAccessed, upgradeData.upgradedVariableName, upgradeData.variableAmount, upgradeData.wholeIncrements, upgradeData.max, upgradeData.minimum))
                    {

                        satisfiedConiditions++;

                    }

                }

                if (satisfiedConiditions == upgrade.upgradeData.Count)
                { 

                    upgradesChecked.Add(upgrade);

                }
            }

        }


        return upgradesChecked;

    }

    public bool CheckLimitations(object target, string variableName, double newValue, bool wholeIncrements, double upperLimit = Mathf.Infinity, double lowerLimit = Mathf.NegativeInfinity)
    {
        
        // Get the type of the target object
        Type targetType = target.GetType();

        // Find the field by name
        FieldInfo fieldInfo = targetType.GetField(variableName, BindingFlags.Public | BindingFlags.Instance);

        bool accetableChange = false;
        // Check if the field was found and is not null
        if (fieldInfo != null)
        {


            double fieldValue = Convert.ToDouble(fieldInfo.GetValue(target));
            double fieldChangedValue = 0;
            if (wholeIncrements)
            {
                fieldChangedValue = fieldValue + newValue;

            }


            else
            {

                fieldChangedValue = fieldValue + (fieldValue * newValue);

            }

            
            if (fieldChangedValue < upperLimit && fieldChangedValue > lowerLimit)
            {

                accetableChange = true;


            }


            // Set the new value to the field
            
        }
        else
        {
            Debug.LogError($"Field '{variableName}' not found in {targetType.Name}.");
        }

        return accetableChange;
    }




    public IEnumerator LevelUpCall()
    {
        upgradePanel.GetComponent<Image>().enabled = true;
        upgradeDescriptions.SetActive(true);
        int playerLevel = PlayerStats.PlayerStatsGlobal.playerLevel;
        List<GameObject> buttonList = new List<GameObject>();

        upgradesAvailable.Clear();
        upgradesSelected.Clear();
        upgradesAvailable = CheckAvailableUpgrades();
        maxAvailableUpgrades = Convert.ToInt32(MathF.Min(maxAvailableUpgrades, upgradesAvailable.Count));



        while (upgradesSelected.Count < maxAvailableUpgrades)
        {

            int randomSelection = UnityEngine.Random.Range(0, upgradesAvailable.Count);
            if (!upgradesSelected.Contains(upgradesAvailable[randomSelection]))
            {

                upgradesSelected.Add(upgradesAvailable[randomSelection]);


            }

        }



        for (int run = 0; run < upgradesSelected.Count; run++)
        {

            GameObject upgradeButton = Instantiate(upgradeButtonPrefab, upgradePanel.transform, false);
            buttonList.Add(upgradeButton);
            upgradeButton.GetComponent<UpgradeButton>().upgrade = upgradesSelected[run];
            upgradeButton.GetComponent<UpgradeButton>().name = name;
            upgradeButton.GetComponent<UpgradeButton>().description = description;
            upgradeButton.GetComponent<Image>().sprite = upgradesSelected[run].icon;
        }



        Time.timeScale = 0;

        yield return new WaitUntil(() => PlayerStats.PlayerStatsGlobal.playerLevel > playerLevel);

        Time.timeScale = 1;

        for (int run = 0; run < buttonList.Count; run++)
        {
            if(buttonList[run].GetComponent<UpgradeButton>().selected)
            {

                upgrades.Find(Upgrade => Upgrade.name == buttonList[run].GetComponent<UpgradeButton>().upgrade.name).allowedSelections--;

            }
            Destroy(buttonList[run]);
             
            
        }


        upgradePanel.GetComponent<Image>().enabled = false;
        upgradeDescriptions.SetActive(false);

        yield break;


    }

}
