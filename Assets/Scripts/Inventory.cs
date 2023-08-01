using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Event that is triggered when the coin count in the inventory changes
    public event Action OnCoinCountChange;

    // The current number of coins in the inventory
    [SerializeField] int coinCountInventory = 0;

    // The base number of coins at the start of the game (can be set in the Inspector)
    [SerializeField] int coinCountBase = 0;

    // Method to add a coin to the inventory
    public void AddCoin()
    {
        coinCountInventory++;
        // Invoke the OnCoinCountChange event to notify listeners about the change
        OnCoinCountChange?.Invoke();
    }

    // Method to take a coin from the inventory
    public void TakeCoin()
    {
        coinCountInventory--;
        // Invoke the OnCoinCountChange event to notify listeners about the change
        OnCoinCountChange?.Invoke();
    }

    // Method to get the current coin count in the inventory
    public int GetCoinCountInventory()
    {
        return coinCountInventory;
    }

    // Method to clear the entire inventory (reset coin count to 0)
    public void ClearInventory()
    {
        coinCountInventory = 0;
        // Invoke the OnCoinCountChange event to notify listeners about the change
        OnCoinCountChange?.Invoke();
    }
}
