﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float speed;
    public Rigidbody2D rb;
    public float lifetime;
    public int damageToGive;
    Animator animator;
    PlayerController thePlayer;

    // Start is called before the first frame update
    void Start()
    {

        thePlayer = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
        //...setting shoot direction
        Vector3 shootDirection;
        shootDirection = Input.mousePosition;
        shootDirection.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        shootDirection = shootDirection - transform.position;

        rb.velocity = new Vector2(shootDirection.x * speed, shootDirection.y * speed);

        Physics2D.IgnoreCollision(thePlayer.gameObject.GetComponent<BoxCollider2D>(), gameObject.GetComponent<CircleCollider2D>());

    }

    void Update()
    {
        //Destroys bullet after set time
        lifetime -= Time.deltaTime; 

        if (lifetime <= 0)
        {
            DestroyBullet();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
        //Deals damage to enemy and destroys bullet
        if (enemy != null)
        {
            enemy.TakeDamage(damageToGive);
            animator.SetBool("explode",true);
        }
    }

    public void DestroyBullet() { Destroy(gameObject); }
}



 
