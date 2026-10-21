using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RepairPurchaseDemo : MonoBehaviour
{

    [SerializeField] private int startingCoins = 6;
    [SerializeField] private int repairCost = 2;
    [SerializeField] private int strongRepairCost = 3;
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

        // adding a new button to use a different tool (strong) that has a different repair amount
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null && keyboard.eKey.wasPressedThisFrame)
        {
            PurchaseSmallRepair();
        }
//here:
        if (keyboard != null && keyboard.qKey.wasPressedThisFrame)
        {
            PurchaseStrongRepair();
        }

        //end

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
    if (crate != null)
    {
        crate.DurabilityChanged -= HandleDurabilityChanged;
    }

        wallet = new Wallet(startingCoins);
        wallet.CoinsChanged += HandleCoinsChanged;
        wallet.CoinsChanged += HandleLowBalance;
        toolbox = new Toolbox();
        toolbox.StoreTool("small", new RepairTool(1));
        //new tool creation
        toolbox.StoreTool("strong", new RepairTool(2));

        crate = new Crate();
        crate.DurabilityChanged += HandleDurabilityChanged;
        crate.TakeDamage(4);
        lastMessage = "Press E to repair.";
        ShowCoins();
        messageTimeRemaining = 0f;
        RefreshDisplay();
    }

    private void HandleDurabilityChanged(int newDurability)
    {
        Debug.Log("Durability event: " + newDurability);
    }

    private void HandleCoinsChanged(int newBalance)
    {
        Debug.Log("Event received. Coins: " + newBalance);
        RefreshDisplay();
    }


    //test

    [ContextMenu("Grant 5 Coins")]
    private void GrantFiveCoins()
    {
        if (wallet == null)
        {
            return;
        }

        wallet.TryAddCoins(5);
    }

    //temp 


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
        int previousDurability = crate.Durability;
        crate.TakeDamage(1);
        if (crate.Durability == previousDurability)
        {
            lastMessage = "No damage applied.";
        }
        else
        {
            lastMessage = "Crate damaged!";
        }
        messageTimeRemaining = messageDuration;
        RefreshDisplay();    
    }

    
    [ContextMenu("Check Crate Damage")]
    private void CheckCrateDamage()
    {
        Crate testCrate = new Crate();
        int changeCount = 0;
        int lastReportedDurability = -1;

        testCrate.DurabilityChanged += newDurability =>
        {
            changeCount++;
            lastReportedDurability = newDurability;
        };

        testCrate.TakeDamage(10);

        Debug.Assert(testCrate.Durability == 0, "Expected durability to be 0 after taking 10 damage.");
        Debug.Assert(changeCount == 1, "Expected one DurabilityChanged event to be raised.");
        Debug.Assert(lastReportedDurability == 0, "Expected last reported durability to be 0.");

        testCrate.TakeDamage(1);

        Debug.Assert(testCrate.Durability == 0, "Expected durability to remain 0 after taking additional damage.");
        Debug.Assert(changeCount == 1, "Expected no additional DurabilityChanged events to be raised.");

        Debug.Log("Crate damage checks finished.");
    }

    private void OnDestroy()
    {
        if (wallet != null)
        {
            wallet.CoinsChanged -= HandleCoinsChanged;
            wallet.CoinsChanged -= HandleLowBalance;
        }
        if (crate != null)
        {
            crate.DurabilityChanged -= HandleDurabilityChanged;
        }
    }


    // updating purchase repair so it can work with a new tool that has a different repair amount (strong)  
    public void PurchaseRepair(string toolName, int repairCost)
    {
        RepairPurchaseResult result = TryPurchaseRepair(
            wallet, toolbox, toolName, crate, repairCost);

        lastMessage = GetResultMessage(result);
        messageTimeRemaining = messageDuration;
        Debug.Log(lastMessage);
        ShowCoins();
        Debug.Log("Durability: " + crate.Durability);
        RefreshDisplay();
        
    }
//added 2 methods for different repairs 
    [ContextMenu("Purchase Repair")]
    public void PurchaseSmallRepair()
    {
        PurchaseRepair("small", repairCost);
    }

    public void PurchaseStrongRepair()
    {
        PurchaseRepair("strong", strongRepairCost);
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
//changed trygettool("small"... to toolName...) 
    string toolStatusOne = "Tool missing";
    string toolStatusTwo = "Tool missing";
    if (toolbox.TryGetTool("small", out RepairTool selectedTool))
        {
            toolStatusOne = "Tool 1 uses: " + selectedTool.UsesRemaining;
        }

    if (toolbox.TryGetTool("strong", out RepairTool selectedToolTwo))
        {
            toolStatusTwo = "Tool 2 uses: " + selectedToolTwo.UsesRemaining;
        }


statusText.text =
    "Coins: " + wallet.Coins +
    "\nDurability: " + crate.Durability +
    "\n" + toolStatusOne +
    "\n" + toolStatusTwo +
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
