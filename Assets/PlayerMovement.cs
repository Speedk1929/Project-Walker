using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Transform playerTransform;
    Rigidbody2D rigidbody2D;
    PlayerControls inputActions = null;

    PlayerStats playerStats;
    public double currentSpeed;
    public Transform firePoint;
    public GameObject bullet;

    // Start is called before the first frame update

    private void Awake()
    {


        playerTransform = transform;
        TryGetComponent(out rigidbody2D);
        TryGetComponent(out playerStats);


        inputActions = new PlayerControls();



    }



    private void OnEnable()
    {

        inputActions.Enable();

    }

    private void OnDisable()
    {

        inputActions.Disable();

    }





    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 lookDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position).normalized;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        // Rotate the sprite to face towards the mouse
        transform.rotation = Quaternion.Euler(0f, 0f, angle -90);


        Vector2 movementCommands = inputActions.Player.Movement.ReadValue<Vector2>();

        
        float speed = Mathf.Abs(rigidbody2D.velocity.x) + Mathf.Abs(rigidbody2D.velocity.y);
        currentSpeed = Mathf.Abs(rigidbody2D.velocity.x) + Mathf.Abs(rigidbody2D.velocity.y);






        rigidbody2D.AddForce(playerStats.acceleration * movementCommands * Time.deltaTime * 100);
        
        if (rigidbody2D.velocity.magnitude > playerStats.maxSpeed)
        {
            
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * playerStats.maxSpeed;
        }

        if (movementCommands.magnitude == 0)
        {

            rigidbody2D.velocity = rigidbody2D.velocity.normalized * 2;
            rigidbody2D.drag = 10;

        }
        if (movementCommands.magnitude != 0)
        {

            rigidbody2D.drag = 0;


        }

        if (playerStats.fireRateCountdown <= 0 && inputActions.Player.Shooting.ReadValue<float>() != 0)
        {

            Fire();
           

        }



    }

    public void Fire()
    {

        playerStats.fireRateCountdown = playerStats.fireRate;
        GameObject bulletFired = Instantiate(bullet, null, false);
        bulletFired.transform.position = firePoint.position;
        bulletFired.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles);


    }



}
