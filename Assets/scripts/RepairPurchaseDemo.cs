using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RepairPurchaseDemo : MonoBehaviour
{

    [SerializeField] private int startingCoins = 6;
    [SerializeField] private int repairCost = 2;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private float messageDuration = 3f;
    private float messageTimeRemaining; 
    private Wallet wallet;
    private Toolbox toolbox;
    private Crate crate;
    private string lastMessage = "Press E to repair.";

    private void Start()
    {
        ResetDemo();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null && keyboard.eKey.wasPressedThisFrame)
        {
            PurchaseRepair();
        }

        if (messageTimeRemaining > 0f)
        {
            messageTimeRemaining -= Time.deltaTime;

            if (messageTimeRemaining <= 0f)
            {
                lastMessage = "Press E to repair.";
                RefreshDisplay();
            }
        }

    }

    public void ResetDemo()
    {
        if (wallet != null)
    {
        wallet.CoinsChanged -= HandleCoinsChanged;
        wallet.CoinsChanged -= HandleLowBalance;
    }

        wallet = new Wallet(startingCoins);
        wallet.CoinsChanged += HandleCoinsChanged;
        wallet.CoinsChanged += HandleLowBalance;
        toolbox = new Toolbox();
        toolbox.StoreTool("small", new RepairTool(1));
        crate = new Crate();
        crate.TakeDamage(4);
        lastMessage = "Press E to repair.";
        ShowCoins();
        messageTimeRemaining = 0f;
        RefreshDisplay();
    }


    private void HandleCoinsChanged(int newBalance)
    {
        Debug.Log("Event received. Coins: " + newBalance);
    }

    private void HandleLowBalance(int newBalance)
    {
        if (newBalance < repairCost)
        {
            Debug.Log("Not enough coins!");
        }
    }

    public void CollectCoin()
    {
        wallet.TryAddCoins(1);
        lastMessage = "Coin 1 collected!";
        messageTimeRemaining = messageDuration;
        RefreshDisplay();
    }

[ContextMenu("Check Wallet")]
private void CheckWallet()
{
    Wallet testWallet = new Wallet(5);
    int changeCount = 0;

testWallet.CoinsChanged += newBalance =>
{
    changeCount++;
};

    bool spent = testWallet.TrySpend(2);

    Debug.Assert(spent, "Spending 2 from 5 should succeed.");
    Debug.Assert(testWallet.Coins == 3, "Expected 3 coins.");
    Debug.Assert(changeCount == 1,
    "A successful spend should raise one event.");

    bool secondSpend = testWallet.TrySpend(4);
    Debug.Assert(!secondSpend, "Spending 4 from 3 should fail.");
    Debug.Assert(testWallet.Coins == 3, "Expected 3 coins after failed spend.");
    Debug.Assert(changeCount == 1,
    "A rejected spend should not raise another event.");

    bool exactSpend = testWallet.TrySpend(3);
    Debug.Assert(exactSpend, "Spending 3 from 3 should succeed.");
    Debug.Assert(testWallet.Coins == 0, "Expected 0 coins after exact spend.");
    Debug.Assert(changeCount == 2,
    "The second successful spend should raise another event.");

    bool zeroSpend = testWallet.TrySpend(0);
    Debug.Assert(!zeroSpend, "Spending 0 should fail.");
    Debug.Assert(testWallet.Coins == 0, "Expected 0 coins after failed zero spend.");

    Debug.Assert(changeCount == 2, "Expected 2 CoinsChanged events to be raised.");

    Debug.Log("Wallet checks finished.");
}

[ContextMenu("Check Coin Collection")]
private void CheckCoinCollection()
{
    Wallet testWallet = new Wallet(1);
    int changeCount = 0;

    testWallet.CoinsChanged += newBalance =>
    {
        changeCount++;
    };

    bool added = testWallet.TryAddCoins(3);
    Debug.Assert(added, "Adding 3 coins should succeed.");
    Debug.Assert(testWallet.Coins == 4, "Expected 4 coins after adding.");
    Debug.Assert(changeCount == 1, "Adding coins should raise one event.");

    bool zeroAdd = testWallet.TryAddCoins(0);
    Debug.Assert(!zeroAdd, "Adding zero coins should fail.");
    Debug.Assert(testWallet.Coins == 4, "Expected 4 coins after failed zero add.");
    Debug.Assert(changeCount == 1, "Failed zero add should not raise another event.");

    bool negativeAdd = testWallet.TryAddCoins(-2);
    Debug.Assert(!negativeAdd, "Adding negative coins should fail.");
    Debug.Assert(testWallet.Coins == 4, "Expected 4 coins after failed add.");
    Debug.Assert(changeCount == 1, "Failed add should not raise another event.");

    Debug.Log("Coin collection checks finished.");
}

    public void DamageCrate()
    {
        crate.TakeDamage(1);
        lastMessage = "Crate damaged!";
        messageTimeRemaining = messageDuration;
        RefreshDisplay();
        
    }
private void OnDestroy()
{
    if (wallet != null)
    {
        wallet.CoinsChanged -= HandleCoinsChanged;
        wallet.CoinsChanged -= HandleLowBalance;
    }
}

    [ContextMenu("Purchase Repair")]
    public void PurchaseRepair()
    {
        RepairPurchaseResult result = TryPurchaseRepair(
            wallet, toolbox, "small", crate, repairCost);

        lastMessage = GetResultMessage(result);
        messageTimeRemaining = messageDuration;
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

    string toolStatus = "Tool missing";
    if (toolbox.TryGetTool("small", out RepairTool selectedTool))
        {
            toolStatus = "Tool uses: " + selectedTool.UsesRemaining;
        }


statusText.text =
    "Coins: " + wallet.Coins +
    "\nDurability: " + crate.Durability +
    "\n" + toolStatus +
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
