using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    public GameObject life;
    public Sprite fullLife;
    public Sprite emptyLife;
    public List<GameObject> lives = new List<GameObject>();

    public GameObject healthBar;


    private void Update()
    {


        FindHealthAmount();



    }


    public void FindHealthAmount()
    {
        if (lives.Count != PlayerStats.PlayerStatsGlobal.maxHealth)
        {


            for (int run = 0; run < lives.Count; run++)
            {

                Destroy(lives[run]);


            }

            lives.Clear();

            for (int run = 0; run < PlayerStats.PlayerStatsGlobal.maxHealth; run++)
            {

                if (lives.Count < PlayerStats.PlayerStatsGlobal.maxHealth)
                {

                    GameObject newLife = Instantiate(life, healthBar.transform);
                    lives.Add(newLife);



                }
            }

        }

        int fullLives = Convert.ToInt32(PlayerStats.PlayerStatsGlobal.currentHealth);



        foreach (GameObject lifeIcon in lives)
        {
            fullLives--;

            if (fullLives >= 0 )
            {

                lifeIcon.GetComponent<Image>().sprite = fullLife;

            }
            else
            {

                lifeIcon.GetComponent<Image>().sprite = emptyLife;


            }
            

        }

    }




}
