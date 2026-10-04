using UnityEngine;

public class Wallet
{
    private int coins;

    public Wallet (int startingCoins)
    {
        coins = startingCoins;
    }

    public int Coins => coins;
    public event System.Action CoinsChanged;

    public bool TrySpend(int amount)
    {
        if (!CanAfford(amount))
        {
            return false;
        }

        coins -= amount;
        CoinsChanged?.Invoke(); 
        return true;
    }


    public bool CanAfford(int amount)
    {
        return amount > 0 && coins >= amount;
    }

}
