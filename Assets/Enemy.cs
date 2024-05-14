using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

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
    GameObject player;

    [Header("Reward Parameters")]
    [Range(0, 100)]
    public double expirience = 1;

    [Header("Spawn Properties")]
    public int spawnCost = 1;
    public bool apartOfWave = false;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        TryGetComponent(out pathing);
        TryGetComponent(out destination);
        pathing.maxSpeed = Convert.ToSingle(speed);
        pathing.maxAcceleration = Convert.ToSingle(maxAcceleration);
        destination.target = player.transform;




    }



    public void TakeDamage(double damage)
    {

        

        health -= damage;
        if (health <= 0)
        {

            player.GetComponent<PlayerStats>().AddExpirience(expirience);
            Destroy(gameObject);

        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {

            player.GetComponent<PlayerStats>().TakeDamage(damage, transform.position);




        }


    }


}
