﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item_SO item;

    public List<Item_SO> stackedItems = new List<Item_SO>();
    private List<Item_SO> stackedItemsTemp = new List<Item_SO>();

    public TextMeshProUGUI stackNumber;

    private Image spriteImage;
    private UIItem selectedItem;
    private Tooltip tooltip;

    private void Awake()
    {
        spriteImage = GetComponent<Image>();
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
        tooltip = FindObjectOfType<Tooltip>();
    }

    public void UpdateItem(Item_SO item)
    {
        this.item = item;

        if (item != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = this.item.ItemSprite;
            if (item.IsStackable)
            {
                
                stackNumber.enabled = true;
            }
            else
            {
                stackNumber.enabled = false;
            }
        }
        else
        {
            spriteImage.color = Color.clear;
            spriteImage.sprite = null;
            stackNumber.enabled = false;
        }
    }

    public void AddToStack(Item_SO item)
    {
        stackedItems.Add(item);
        stackNumber.text = (stackedItems.Count).ToString();
    }

    public void RemoveFromStack(Item_SO item)
    {
        if (stackedItems.Count > 1)
        {
            stackedItems.RemoveAt(stackedItems.FindIndex(i => i == item));
        }
        else
        {
            ResetItemStack();
        }

        stackNumber.text = (stackedItems.Count).ToString();
    }

    public void ResetItemStack()
    {
        stackedItems.Clear();
        UpdateItem(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.item != null)
        {
            tooltip.gameObject.GetComponent<Image>().enabled = true;
            tooltip.GenerateTooltip(this.item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.GetComponent<Image>().enabled = false;
    }
    
    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.item != null) // if slot is empty or not 
        {
            if (Input.GetKey(KeyCode.LeftShift) && item.IsStackable)
            {
                if (selectedItem.item != null) // if selected item is empty or not
                {
                    if (selectedItem.item.IsStackable)
                    {
                        AddOneStackedItemToSelectedItem(); // adds one stacked item to selected and removes one from this stack
                    }
                }
                else
                {
                    // if selected item is empty and the item is stackable
                    selectedItem.UpdateItem(this.item); // update selectedItem with this item
                    AddOneStackedItemToSelectedItem(); // adds one stacked item to selected and removes one from this stack
                }
            }
            else
            {
                if (selectedItem.item != null)
                {
                    if (this.item.ItemID == selectedItem.item.ItemID && this.item.IsStackable)
                    {
                        for (int i = 0; i < selectedItem.stackedItems.Count; i++)
                        {
                            print(selectedItem.stackedItems.Count);
                            this.AddToStack(selectedItem.stackedItems[i]);
                            selectedItem.RemoveFromStack(selectedItem.stackedItems[i]);
                        }
                    }
                    else
                    {
                        Swap();
                    }
                }
                else
                {
                    PickUp();
                }
            }
        }
        else if (selectedItem.item != null) // if slot is empty, but selected is not
        {
            if (Input.GetKey(KeyCode.LeftShift) && item.IsStackable)
            {
                
            }
            else
            {
                Place();
            }
        }
    }
    */

    public void OnPointerClick(PointerEventData eventData)
    {
        /* METHODS:
            - PickUp(); - Picks up a nonstackable item and updates both this item and selected item.
            - PickUpOneItem(); - Picks up one item in stack list, adds to selected item and removes from this item stack
            - PickUpAllItems(); - Picks up all items in stack list, adds them to selected items stack and removes from this item stack. Updates this and selected item with item
            - Swap(); - Swaps if item.itemID is equal to selected.item.itemID
            - Place(); - Places item and items in stack, updates this item and selected item
            - AddStackFromSelected(); - Adds items from selected stack to this item stack, but stops if this item stack is full
            - AddOneItemToStackFromSelected(); - Adds one item from selected list to this list
         */

        if (this.item != null) // if slot is empty or not 
        {
            if (selectedItem.item != null) // if selected slot is empty or not
            {
                if (this.item.IsStackable && selectedItem.item.IsStackable) // if both the selected item and item in slot is stackable
                {
                    if (this.item.ItemID == selectedItem.item.ItemID)
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            if(selectedItem.stackedItems.Count < this.item.MaxStack)
                            {
                                PickUpOneItem(); // picks up one item until the stack is full
                            }
                        }
                        else
                        {
                            PlaceItemsFromSelectedStack(); // Adds items to this item stack until its full
                        }
                    }
                    else
                    {
                        Swap(); // Swaps items with selected items if doesn't match itemID
                    }
                }
                else
                {
                    Swap(); // Swaps item with selected item, either if it stackable or not
                }
            }
            else 
            {
                if (eventData.button == PointerEventData.InputButton.Middle)
                {
                    PromptForDeletion();
                }
                else
                {
                    if (this.item.IsStackable) // if the item you are picking up is stackable
                    {
                        if (Input.GetKey(KeyCode.LeftShift)) // If shift is pressed, then pick up one on each click
                        {
                            PickUpOneItem();
                        }
                        else // if shift is not detected, pick up all
                        {
                            PickUpAllItems(); // picks up all stacked items and clears this item stack
                        }
                    }
                    else
                    {
                        PickUp(); // picks up non stackable item
                    }
                }
            }
        }
        else if (selectedItem.item != null) // if slot is empty, but selected is not
        {
            if (selectedItem.item.IsStackable)
            {
                PlaceItemsFromSelectedStack(); // Places all selected items in this item
            }
            else
            {
                Place(); // Places the single item in empty slot
            }

            
        }

        
    }

    private void PromptForDeletion()
    {
        UIController uiController = FindObjectOfType<UIController>();
        string text = null;

        if (item.IsStackable)
        { text = string.Format("Are you sure you want to delete \n {0} x {1} ?",stackedItems.Count, item.ItemTitle); }
        else
        { text = string.Format("Are you sure you want to delete \n {0} ?", item.ItemTitle); }
        
        uiController.LoadConfirmationDialouge(ConfirmationWindow.ConfirmationType.DeleteItem, text, item, stackedItems.Count);
    }

    private void PlaceOneItemFromSelectedStack(int index)
    {
        if(this.stackedItems.Count < item.MaxStack)
        {
            this.AddToStack(selectedItem.stackedItems[index]);
            selectedItem.RemoveFromStack(selectedItem.stackedItems[index]);
        }
    }

    private void PlaceItemsFromSelectedStack()
    {
        UpdateItem(selectedItem.item);
        int itemsAdded = 0;

        for (int i = 0; i < selectedItem.stackedItems.Count; i++)
        {
            if (this.stackedItems.Count >= this.item.MaxStack)
            {
                break;
            }
            else
            {
                itemsAdded++;
                this.AddToStack(selectedItem.stackedItems[i]);
            }    
        }
        UpdateItem(selectedItem.item);

        if (selectedItem.stackedItems.Count < 1)
        {
            selectedItem.ResetItemStack();
        }
        else
        {
            for (int i = 0; i < itemsAdded; i++)
            {
                selectedItem.RemoveFromStack(selectedItem.stackedItems[selectedItem.stackedItems.Count -1]);
            }

            if (selectedItem.stackedItems.Count < 1)
            {
                selectedItem.ResetItemStack();
            }
        }
    }

    private void PickUpAllItems()
    {
        // Picks up all items in a stack, adds them to the selected item, removed from item stack.
        
        for (int i = 0; i < this.stackedItems.Count; i++)
        {
            selectedItem.AddToStack(this.stackedItems[i]);
        }
        selectedItem.UpdateItem(this.item);
        this.stackedItems.Clear();
        UpdateItem(null);
    }

    private void PickUpOneItem()
    {
        // picks up one item and adds to selected stack, removes one from this item stack.
   
        if (selectedItem.stackedItems.Count < 1)
        {
            selectedItem.UpdateItem(this.item);
        }
        selectedItem.AddToStack(this.item);
        this.RemoveFromStack(this.item);

    }

    private void Place() // Places the selected item to the selected slot
    {
        UpdateItem(selectedItem.item);
        selectedItem.stackedItems.Clear();
        selectedItem.UpdateItem(null);
    }

    private void PickUp() // This will pick up selected item
    {
        selectedItem.UpdateItem(this.item);
        UpdateItem(null);
    }

    private void Swap() // This will replace the object you are holding with the one in slot
    {
        Item_SO clone = Item_SO.CreateInstance(selectedItem.item);

        for (int i = 0; i < selectedItem.stackedItems.Count; i++) // Adds selected.stackeditems to temp list
        {
            stackedItemsTemp.Add(selectedItem.stackedItems[i]);
        }

        selectedItem.ResetItemStack(); // Empties selected item stack

        for (int i = 0; i < this.stackedItems.Count; i++) // Adds this.stackeditems to selected
        {
            selectedItem.AddToStack(this.stackedItems[i]);
        }
        selectedItem.UpdateItem(this.item);

        this.ResetItemStack(); // Empties this item stack

        for (int i = 0; i < this.stackedItemsTemp.Count; i++) // Adds tempItems to stacked items list
        {
            this.AddToStack(this.stackedItemsTemp[i]);
        }

        UpdateItem(clone);

        this.stackedItemsTemp.Clear(); // Empties the temp item stack
    }

    private bool IsPointerOverUIObject()
    {
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
