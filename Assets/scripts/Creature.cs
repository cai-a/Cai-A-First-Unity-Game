using UnityEngine;

public abstract class Creature : IDamageable

{
    public abstract string Describe();
    protected int health = 10;

    public int Health => health;

    public virtual void TakeDamage(int amount)
    {
        health -= amount;

        health = System.Math.Max(health, 0);
    }

}