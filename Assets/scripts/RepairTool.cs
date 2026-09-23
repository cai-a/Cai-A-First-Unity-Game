using UnityEngine;

public class RepairTool
{
    private int repairAmount;
    private int usesRemaining = 2;

    public int UsesRemaining => usesRemaining;

    public RepairTool(int repairAmount)
    {
        this.repairAmount = repairAmount;
    }

    public bool Use(IRepairable target)
    {
        if (target == null || usesRemaining <= 0)
        {
            UnityEngine.Debug.Log("Repair unavailable.");
            return false;
        }

        bool repaired = target.Repair(repairAmount);

        if (!repaired)
        {
            UnityEngine.Debug.Log("Nothing repaired. Use saved.");
            return false;
        }

        usesRemaining--;

        UnityEngine.Debug.Log("Uses left: " + usesRemaining);
        return true;
    }
}