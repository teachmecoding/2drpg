﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroDetection : MonoBehaviour
{
    EnemyController parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            GetComponentInParent<NPCMovementController>().StartChase(collision.gameObject.transform);
        }
        
        /*
        if (player != null && parent.idle && !parent.isDead)
        {
            parent.InitiateChase(collision.gameObject);
        }
        */
    }
}
