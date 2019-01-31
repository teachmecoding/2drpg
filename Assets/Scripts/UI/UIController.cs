﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIController : MonoBehaviour
{
    PlayerController playerController;
    public GameObject shopController;
    public GameObject inventoryPanel;
    public GameObject inventorySlots;
    public GameObject characterPanel;
    public GameObject testButtons;
    public GameObject confirmationDialogue;
    public GameObject dialoguePanel;

    public Text playerHP;

    public GameObject deathMenu;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        deathMenu.SetActive(false);
        inventoryPanel.SetActive(false);
        inventorySlots.SetActive(false);
        characterPanel.SetActive(false);
        testButtons.SetActive(false);
        confirmationDialogue.SetActive(false);
        dialoguePanel.SetActive(false);
        shopController.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryUI();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCharacterPanel();
        }

        playerHP.text = "HP: " + playerController.player_HealthPoints.ToString();
    }

    private void ToggleCharacterPanel()
    {
        characterPanel.SetActive(!characterPanel.activeSelf);
    }

    public void ToggleInventoryUI()
    {
        inventoryPanel.SetActive(!inventoryPanel.gameObject.activeSelf);
        inventorySlots.SetActive(!inventorySlots.activeSelf);
        testButtons.SetActive(!testButtons.activeSelf);
    }

    public GameObject OpenDialoguePanel()
    {
        dialoguePanel.SetActive(true);
        return dialoguePanel;
    }

    public GameObject OpenShop()
    {
        shopController.SetActive(true);
        return shopController;
    }

    private void FixedUpdate()
    {
        if (playerController.isDead)
        {
            deathMenu.SetActive(true);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadConfirmationDialouge(string dialougeInput)
    {
        confirmationDialogue.SetActive(true);
        confirmationDialogue.GetComponent<ConfirmationWindow>().ConfirmationDialogue(dialougeInput);
    }
}
