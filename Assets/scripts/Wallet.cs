using UnityEngine;

public class Wallet
{
    private int coins;

    public Wallet (int startingCoins)
    {
        coins = startingCoins;
    }

    public int Coins => coins;
    public event System.Action<int> CoinsChanged;

    public bool TrySpend(int amount)
    {
        if (!CanAfford(amount))
        {
            return false;
        }

        coins -= amount;
        CoinsChanged?.Invoke(coins);
        return true;
    }
    public bool TryAddCoins(int amount)
    {
        if (amount <= 0)
        {
            return false;
        }

        coins += amount;
        CoinsChanged?.Invoke(coins);
        return true;
    }

    public bool CanAfford(int amount)
    {
        return amount > 0 && coins >= amount;
    }

}
