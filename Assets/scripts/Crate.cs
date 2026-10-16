using UnityEngine;

public class Crate : IDamageable, IRepairable

{
    public event System.Action<int> DurabilityChanged;
    private int durability = 5;
    public int Durability => durability;
    public void TakeDamage(int amount)
    {
        if (amount <= 0 || durability <= 0)
        {
            return;
        }

        durability = System.Math.Max(durability - amount, 0);
        DurabilityChanged?.Invoke(durability);
        UnityEngine.Debug.Log("Crate durability: " + durability);
    }

    public bool Repair(int amount)
    {
        if (durability >= 5 || amount <= 0)
        {
            return false;
        }

        durability = System.Math.Min(durability + amount, 5);
        DurabilityChanged?.Invoke(durability);

        UnityEngine.Debug.Log("Crate durability: " + durability);

        return true;
    }
}