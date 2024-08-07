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

    public int maxAvaliableUpgrades = 4;
    public GameObject upgradeButtonPrefab;
    public GameObject upgradePanel;
    public GameObject upgradeDescriptions;

    public TextMeshProUGUI name;
    public TextMeshProUGUI description;



    private void Start()
    {

        maxAvaliableUpgrades = Convert.ToInt32(MathF.Min(maxAvaliableUpgrades, upgrades.Count));
        PlayerStats.PlayerStatsGlobal.upgrade += LevelUp;
        upgradePanel.GetComponent<Image>().enabled = false;
        upgradeDescriptions.SetActive(false);
    }


    public void LevelUp()
    {

        StartCoroutine(LevelUpCall());

    }



    public IEnumerator LevelUpCall()
    {
        upgradePanel.GetComponent<Image>().enabled = true;
        upgradeDescriptions.SetActive(true);
        int playerLevel = PlayerStats.PlayerStatsGlobal.playerLevel;
        List<GameObject> buttonList = new List<GameObject>();

        while (upgradesSelected.Count < maxAvaliableUpgrades)
        {

            int randomSelection = UnityEngine.Random.Range(0, upgrades.Count);
            if (!upgradesSelected.Contains(upgrades[randomSelection]))
            {

                upgradesSelected.Add(upgrades[randomSelection]);


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

            Destroy(buttonList[run]);
             
            
        }


        upgradePanel.GetComponent<Image>().enabled = false;
        upgradeDescriptions.SetActive(false);

        yield break;


    }

}
