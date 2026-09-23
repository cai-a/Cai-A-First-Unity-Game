using UnityEngine;

public class RepairPurchaseDemo : MonoBehaviour
{
    private void Start()
    {

    Wallet wallet = new Wallet(6);

    Toolbox toolbox = new Toolbox();
    toolbox.StoreTool("small", new RepairTool(1));

    Crate crate = new Crate();
    crate.TakeDamage(4);

    bool first = TryPurchaseRepair(
        wallet, toolbox, "small", crate, 2);

    bool second = TryPurchaseRepair(
        wallet, toolbox, "small", crate, 2);

    bool third = TryPurchaseRepair(
        wallet, toolbox, "small", crate, 2);

    Debug.Log(first);
    Debug.Log(second);
    Debug.Log(third);
    Debug.Log(wallet.Coins);
    Debug.Log(crate.Durability);

    }


    private bool TryPurchaseRepair(
        Wallet wallet,
        Toolbox toolbox,
        string toolName,
        IRepairable target,
        int cost)
    {

        if (wallet == null || toolbox == null || target == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(toolName))
        {
            return false;
        }
        
        if (wallet.CanAfford(cost) && toolbox.UseTool(toolName, target))
        {
            wallet.TrySpend(cost);
            return true;
        }

        return false;


    } 

}