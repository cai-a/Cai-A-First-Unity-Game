using UnityEngine;

public class Wallet
{
    private int coins;

    public Wallet (int startingCoins)
    {
        coins = startingCoins;
    }

    public int Coins => coins;

    public bool TrySpend(int amount)
    {
        if (!CanAfford(amount))
        {
            return false;
        }

        coins -= amount;
        return true;
    }


    public bool CanAfford(int amount)
    {
        return amount > 0 && coins >= amount;
    }


}
