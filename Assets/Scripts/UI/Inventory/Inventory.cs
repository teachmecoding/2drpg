﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item_SO> characterItems = new List<Item_SO>();
    public ItemDatabase itemDatabase;
    public UIInventory uiInventory;

    private void Start()
    {
        GiveItem(10);      
    }

    public void GiveItem(int id) // Adds item from ItemDatabase to characterItems list by id
    {
        Item_SO itemToAdd = itemDatabase.GetItem(id);
        characterItems.Add(itemToAdd);
        uiInventory.AddNewItem(itemToAdd);
    }

    public void GiveItem(string itemTitle) // Adds item from ItemDatabase to characterItems list by title
    {
        Item_SO itemToAdd = itemDatabase.GetItem(itemTitle);
        characterItems.Add(itemToAdd);
        uiInventory.AddNewItem(itemToAdd);
    }

    public Item_SO CheckForItem(int id) // Checks if characterItems list contains item by id
    {
        return characterItems.Find(item => item.ItemID == id);
    }

    //
    // TODO Check if the characterList contains 2x of same item, how to handle that...
    //
    public void RemoveItem(int id) // If characterItem list contains item, removes it
    {
        Item_SO itemToRemove = CheckForItem(id);
        if (itemToRemove != null)
        {
            characterItems.Remove(itemToRemove);
            uiInventory.RemoveItem(itemToRemove);
        }
    }
}
