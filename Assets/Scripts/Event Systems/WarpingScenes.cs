﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpingScenes : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
                SceneManager.LoadScene(0);
        }
        
        //Debug.Log("An object collided");
        //other.gameObject.transform.position = warpTarget.position;
        //Camera.main.transform.position = warpTarget.position;
    }
}
