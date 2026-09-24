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

    private void Start()
    {

    wallet = new Wallet(startingCoins);

    toolbox = new Toolbox();
    toolbox.StoreTool("small", new RepairTool(1));

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
    private void PurchaseRepair()
    {
        bool purchased = TryPurchaseRepair(
            wallet, toolbox, "small", crate, repairCost);

        Debug.Log("Purchased: " + purchased);
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
        "\nDurability: " + crate.Durability;
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