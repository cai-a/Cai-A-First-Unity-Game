using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RepairPurchaseDemo : MonoBehaviour
{

    [SerializeField] private int startingCoins = 6;
    [SerializeField] private int repairCost = 2;
    [SerializeField] private TMP_Text statusText;
    private Wallet wallet;
    private Toolbox toolbox;
    private Crate crate;
    private string lastMessage = "Press E to repair.";

    private void Start()
    {
    
    wallet = new Wallet(startingCoins);

    toolbox = new Toolbox();
    toolbox.StoreTool("small", new RepairTool(1));
    bool found = toolbox.TryGetTool("small", out RepairTool selectedTool);

Debug.Log(found);

if (found)
{
    Debug.Log(selectedTool.UsesRemaining);
}

    crate = new Crate();
    crate.TakeDamage(4);

    ShowCoins();
    RefreshDisplay();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null && keyboard.eKey.wasPressedThisFrame)
        {
            PurchaseRepair();
        }
    }

    [ContextMenu("Purchase Repair")]
    public void PurchaseRepair()
    {
        RepairPurchaseResult result = TryPurchaseRepair(
            wallet, toolbox, "small", crate, repairCost);

        lastMessage = GetResultMessage(result);
        Debug.Log(lastMessage);
        ShowCoins();
        Debug.Log("Durability: " + crate.Durability);
        RefreshDisplay();
        
    }

    private void ShowCoins()
    {
        Debug.Log("Wallet coins: " + wallet.Coins);
    }

    private void RefreshDisplay()
{
    if (statusText == null)
    {
        return;
    }

statusText.text =
    "Coins: " + wallet.Coins +
    "\nDurability: " + crate.Durability +
    "\n" + lastMessage;
}

    private RepairPurchaseResult TryPurchaseRepair(
        Wallet wallet,
        Toolbox toolbox,
        string toolName,
        IRepairable target,
        int cost)
    {

        if (wallet == null || toolbox == null || target == null || cost <= 0)
        {
            return RepairPurchaseResult.InvalidRequest;
        }

        if (string.IsNullOrWhiteSpace(toolName))
        {
            return RepairPurchaseResult.InvalidRequest;
        }

        if (!toolbox.TryGetTool(toolName, out RepairTool selectedTool))
        {
            return RepairPurchaseResult.ToolMissing;
        }
        
        if (selectedTool.UsesRemaining <= 0)
        {
            return RepairPurchaseResult.ToolEmpty;
        }

        if (!wallet.CanAfford(cost))
        {
            return RepairPurchaseResult.CannotAfford;
        }

        if (!selectedTool.Use(target))
        {
            return RepairPurchaseResult.NothingToRepair;
        }

        wallet.TrySpend(cost);
        return RepairPurchaseResult.Success;

    } 

    private string GetResultMessage(RepairPurchaseResult result)
{
    switch (result)
    {
        case RepairPurchaseResult.Success:
            return "Repair purchased!";

        case RepairPurchaseResult.CannotAfford:
            return "Not enough coins.";

        case RepairPurchaseResult.ToolMissing:
            return "Tool not found.";

        case RepairPurchaseResult.ToolEmpty:
            return "Tool has no uses left.";

        case RepairPurchaseResult.NothingToRepair:
            return "Nothing needs repairing.";

        default:
            return "Repair unavailable.";
    }
}

}
