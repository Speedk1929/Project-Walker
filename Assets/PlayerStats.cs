using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.U2D;
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

    SpriteShapeRenderer spriteRenderer;
    Coroutine pushback;
    int flickers = 0;
    [Range(1, 10)]
    public int allowedFlickers = 0;
    UnityEngine.Color color;
    UnityEngine.Color flickerColor;

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
        TryGetComponent(out spriteRenderer);

        color = spriteRenderer.color;
        flickerColor = new UnityEngine.Color(color.r, color.g, color.b, 0.01f);

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
        if (maxHealth < currentHealth)
        {

            currentHealth = maxHealth;


        }


    }


    public void TakeDamage(double damage, Vector2 impactPoint)
    {

        currentHealth -= damage;

        Vector2 direction = new Vector2(transform.position.x, transform.position.y) - impactPoint ;

        rb2D.AddForce(direction * pushbackOnDamaged, ForceMode2D.Impulse);



        if (currentHealth <= 0)
        {
<<<<<<< Updated upstream

            Destroy(gameObject);
=======
            Flicker();
            currentHealth -= damage;

            Vector2 direction = new Vector2(transform.position.x, transform.position.y) - impactPoint;

            rb2D.AddForce(direction * pushbackOnDamaged, ForceMode2D.Impulse);

            StartCoroutine(Invincibility());

            if (currentHealth <= 0)
            {

                Destroy(gameObject);

            }
>>>>>>> Stashed changes

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

    public void Flicker()
    {
        flickers++;
        spriteRenderer.color = flickerColor;
        Invoke("ResetColor", UnityEngine.Random.Range(0.05f, 0.1f));

    }

    public void ResetColor()
    {
        spriteRenderer.color = color;
        if (flickers <= allowedFlickers)
        {

            Invoke("Flicker", UnityEngine.Random.Range(0.05f, 0.1f));

        }

        else
        {
            spriteRenderer.color = color;
            flickers = 0;

        }
    }


}
