using UnityEngine;

public class BossCreature : Creature
{
    public void DoubleHealth()
    {
        health *= 2;
    }

    public override void TakeDamage(int amount)
    {
        int reducedDamage = System.Math.Max(amount - 1, 0);

        base.TakeDamage(reducedDamage);
    }

    public override string Describe()
    {
        return "Armored boss";
    }

}