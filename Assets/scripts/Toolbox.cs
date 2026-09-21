using System.Collections.Generic;
using UnityEngine;

public class Toolbox
{
    private Dictionary<string, RepairTool> tools =
        new Dictionary<string, RepairTool>();

    public int Count => tools.Count;

    public void StoreTool(string name, RepairTool tool)
    {
        tools[name] = tool;
    }

    public bool UseTool(string name, IRepairable target)
    {
        if (tools.TryGetValue(name, out RepairTool tool))
        {
            return tool.Use(target);
        }
        else
        {
            Debug.Log("Tool not found: " + name);
            return false;
        }
    }

    public bool RemoveTool(string name)
    {
        return tools.Remove(name);
    }

    public bool HasTool(string name)
    {
        return tools.ContainsKey(name);
    }

    public bool TryStoreTool(string name, RepairTool tool)
    {
        if (tool == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }
        
       if (tools.ContainsKey(name))
        {
        return false;
        }
        else
        {
        tools[name] = tool;
        return true;
        }  
       
       
       }


}
