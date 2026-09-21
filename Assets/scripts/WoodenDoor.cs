using UnityEngine;

public class WoodenDoor : IDamageable
{

   private int durability = 12;

    public void TakeDamage(int amount)
    {
        durability = System.Math.Max(durability - amount, 0);

        UnityEngine.Debug.Log("WD durability: " + durability);
    }
}