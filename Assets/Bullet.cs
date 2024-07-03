using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(0, 10)]
    public double damage = 1;
    public int penetrations = 1;
    [Range(1, 100)]
    public float speed = 1;
    [Range(25, 100)]
    public float maxDistance = 50;
    public float currentDistance = 0;


    public Rigidbody2D rb;
    [Header("Death Effects")]
    public GameObject particleSystem;

    private void Start()
    {
        TryGetComponent(out rb);
        rb.AddForce(transform.up * speed);



    }

    private void FixedUpdate()
    {

        currentDistance += (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y)) * Time.deltaTime;

        if (currentDistance > maxDistance)
        {

            Destroy(gameObject);

        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag != "Bullet" && collision.gameObject.tag != "Player")
        {


            if (collision.gameObject.tag == "Enemy")
            {

                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                


            }



            penetrations--;


            if (penetrations <= 0)
            {

                GameObject particle = Instantiate(particleSystem);
                particle.transform.position = transform.position;
                particle.transform.rotation =  transform.rotation;

                Destroy(gameObject);

            }
        }
    }
    





}
