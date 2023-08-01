using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnCoinCountChange;
    [SerializeField] int coinCountInventory = 0;
    [SerializeField] int coinCountBase = 0;

    public void AddCoin()
    {
        coinCountInventory++;
        OnCoinCountChange?.Invoke();
    }

    public void TakeCoin()
    {

        coinCountInventory--;
        OnCoinCountChange?.Invoke();
    }

    public int GetCoinCountInventory()
    {
        return coinCountInventory;
    }

    public void ClearInventory()
    {
        coinCountInventory = 0;
        OnCoinCountChange?.Invoke();
    }
}

