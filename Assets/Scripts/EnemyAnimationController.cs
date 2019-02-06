﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    Animator animator;
    private Rigidbody2D rigidbody2D;
    Vector2 velocity;
    Vector2 positionLastFrame;
    public ParticleSystem lootParticles;
    public ParticleSystem damageTakenParticles;
    EnemyHealthManager enemyHealthManager;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        lootParticles.gameObject.SetActive(false);
        enemyHealthManager = GetComponent<EnemyHealthManager>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        AnimateMovement();
    }

    private void AnimateMovement()
    {
        velocity = rigidbody2D.velocity;
        if (velocity != Vector2.zero && !enemyHealthManager.IsDead)
        {
            animator.SetBool("IsWalking", true);
            animator.SetFloat("WalkingHorizontal", velocity.x);
            animator.SetFloat("WalkingVertical", velocity.y);
        }
        else if (enemyHealthManager.IsDead)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("Dead", true);
            lootParticles.gameObject.SetActive(true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    public void DamageTaken()
    {
        damageTakenParticles.Emit(3);
    }
}
