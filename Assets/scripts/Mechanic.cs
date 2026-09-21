using UnityEngine;

public class Mechanic
{
    private RepairTool tool;

    public Mechanic(RepairTool tool)
    {
        this.tool = tool;
    }

    public void RepairTarget(IRepairable target)
    {
        tool.Use(target);
    }
    
    public void EquipTool(RepairTool newTool)
    {
        tool = newTool;
    }
}