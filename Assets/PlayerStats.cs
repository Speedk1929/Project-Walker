using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Parameters")]
    public double maxHealth = 3;
    public double regenerationRate = 0;
    public double currentHealth = 3;
    [Header("Speed Parameters")]
    [Range(0, 50)]
    public double maxSpeed = 0;
    [Range(0, 10)]
    public float acceleration = 0;

    [Header("Firepower Parameters")]
    [Range(0.01f, 0.5f)]
    public double fireRate = 1;
    public double fireRateCountdown = 0;

    [Header("Damage Parameters")]
    [Range(0f, 100f)]
    public float pushbackOnDamaged = 10f;

    [Header("Upgrade Parameters")]
    public double xp = 0;
    public double xpNeededToLevelUp = 0;
    public int playerLevel = 0;
    public int upgradesLeft = 0;
    Rigidbody2D rb2D;
    public event Action upgrade;

    public Slider xpSlider;

    public static PlayerStats PlayerStatsGlobal = null;

    private void Awake()
    {
        xpSlider = GameObject.Find("XpSlider").GetComponent<Slider>();
        PlayerStatsGlobal = this;


    }

    private void Start()
    {
        TryGetComponent(out rb2D);


    }

    private void FixedUpdate()
    {


        if (fireRateCountdown > 0)
        {

            fireRateCountdown -= Time.deltaTime;

        }


        if (currentHealth < 3)
        {

            currentHealth += (Time.deltaTime * 1) / 60;


        }


    }


    public void TakeDamage(double damage, Vector2 impactPoint)
    {

        currentHealth -= damage;

        Vector2 direction = new Vector2(transform.position.x, transform.position.y) - impactPoint ;

        rb2D.AddForce(direction * pushbackOnDamaged, ForceMode2D.Impulse);



        if (currentHealth <= 0)
        {

            Destroy(gameObject);

        }


    }

    public void AddExpirience(double expirience)
    {

        xp += expirience;

        xpNeededToLevelUp = Mathf.Pow(playerLevel, 2) * 10 + 10;
        xpSlider.maxValue = Convert.ToSingle(xpNeededToLevelUp);
        xpSlider.value = Convert.ToSingle(xp);
        if (xp >= xpNeededToLevelUp )
        {
            xp = 0;
            xpSlider.value = Convert.ToSingle(xp);
            upgrade.Invoke();


        }

    }


}
