using UnityEngine;

public class FragileCreature : Creature
{
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount * 2);
    }


    public override string Describe()
    {
        return "Fragile creature";
    }

}