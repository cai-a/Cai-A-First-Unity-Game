using UnityEngine;

public class EnemyStats
{
    private int health;
    private int damage;

    public int Health => health;
    public int Damage => damage;

    public EnemyStats(int health, int damage)
    {
        this.health = health;
        this.damage = damage;
    }
}