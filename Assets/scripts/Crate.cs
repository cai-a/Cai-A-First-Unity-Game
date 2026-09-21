using UnityEngine;

public class Crate : IDamageable, IRepairable
{
    private int durability = 5;
    public int Durability => durability;
    public void TakeDamage(int amount)
    {
        durability = System.Math.Max(durability - amount, 0);

        UnityEngine.Debug.Log("Crate durability: " + durability);
    }

    public bool Repair(int amount)
    {
        if (durability >= 5 || amount <= 0)
        {
            return false;
        }

        durability = System.Math.Min(durability + amount, 5);

        UnityEngine.Debug.Log("Crate durability: " + durability);

        return true;
    }
}