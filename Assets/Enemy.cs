using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.U2D;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [Range(0, 100)]
    public double health = 1;
    public double damage = 1;

    [Header("Movement Parameters")]
    [Range(0, 100)]
    public double speed = 1;
    [Range(0, 100)]
    public double maxAcceleration = 10;
    AIPath pathing;
    AIDestinationSetter destination;
    Rigidbody2D rigidbody;
    GameObject player;

    [Header("Reward Parameters")]
    [Range(0, 100)]
    public double expirience = 1;

    [Header("Spawn Properties")]
    public int spawnChance = 0;
    public int spawnCost = 1;
    public bool apartOfWave = false;
<<<<<<< Updated upstream
=======

    [Header("Death Effects")]
    public GameObject particleSystem;
    SpriteRenderer spriteRenderer;
    Coroutine pushback;
    int flickers = 0;
    [Range(1, 10)]
    public int allowedFlickers = 0;
    Color color;
    Color flickerColor;
>>>>>>> Stashed changes
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        TryGetComponent(out pathing);
        TryGetComponent(out destination);
        pathing.maxSpeed = Convert.ToSingle(speed);
        pathing.maxAcceleration = Convert.ToSingle(maxAcceleration);
        destination.target = player.transform;

<<<<<<< Updated upstream


=======
        TryGetComponent(out rigidbody);
        transform.GetChild(0).TryGetComponent(out spriteRenderer);

        color = spriteRenderer.color;
        flickerColor = new Color(color.r, color.g, color.b, 0.01f);
        
>>>>>>> Stashed changes

    }

    public void TakeDamage(double damage)
    {

        health -= damage;
<<<<<<< Updated upstream
        if (health <= 0)
        {

            player.GetComponent<PlayerStats>().AddExpirience(expirience);
            Destroy(gameObject);
=======

        if (health <= 0)
        {

            Death();
>>>>>>> Stashed changes

        }
        if (pushback == null)
        {
            pushback = StartCoroutine(Pushback());
        }
        Flicker();
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {

            player.GetComponent<PlayerStats>().TakeDamage(damage, transform.position);




        }


    }


    public IEnumerator Pushback()
    {

        pathing.enabled = false;
       
        yield return new WaitForSeconds(0.9f);

        pathing.enabled = true;
        pushback = null;
        yield break;

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


    public void Death()
    {

        player.GetComponent<PlayerStats>().AddExpirience(expirience);
        Instantiate(particleSystem).transform.position = transform.position;
        Destroy(gameObject);


    }



}
