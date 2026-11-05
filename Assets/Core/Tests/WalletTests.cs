using NUnit.Framework;

public class WalletTests
{
    [Test]
    public void SpendingTwoFromFiveLeavesThree()
    {
        // Arrange: prepare the starting situation.
        Wallet testWallet = new Wallet(5);

        // Act: perform the action we want to test.
        bool spent = testWallet.TrySpend(2);

        // Assert: check the results.
        Assert.That(spent, Is.True);
        Assert.That(testWallet.Coins, Is.EqualTo(3));
    }

    [Test]
    public void SpendingMoreThanBalanceFails()

    {
        Wallet testWallet = new Wallet(3);

        bool spendTooMuch = testWallet.TrySpend(4);
        Assert.That(spendTooMuch, Is.False);
        Assert.That(testWallet.Coins, Is.EqualTo(3));

    }

    [Test]
    public void SpendingExactBalanceLeavesZero()
    {
        Wallet testWallet = new Wallet(3);

        bool spendExact = testWallet.TrySpend(3);
        Assert.That(spendExact, Is.True);
        Assert.That(testWallet.Coins, Is.EqualTo(0));


    }

[TestCase(0)]
[TestCase(-2)]
public void SpendingInvalidAmountFailsWithoutChangingBalance(int amount)
{
    Wallet testWallet = new Wallet(5);

    bool spent = testWallet.TrySpend(amount);

    Assert.That(spent, Is.False);
    Assert.That(testWallet.Coins, Is.EqualTo(5));
}

[Test]
public void SuccessfulSpendRaisesCoinsChangedOnce()
{
    Wallet testWallet = new Wallet(5);
    int changeCount = 0;
    int lastReportedBalance = -1;

    testWallet.CoinsChanged += newBalance =>
    {
        changeCount++;
        lastReportedBalance = newBalance;
    };

    testWallet.TrySpend(2);

    Assert.That(changeCount, Is.EqualTo(1));
    Assert.That(lastReportedBalance, Is.EqualTo(3));
}

}