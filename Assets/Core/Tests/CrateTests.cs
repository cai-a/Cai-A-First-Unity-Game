using NUnit.Framework;

public class CrateTests
{
    [Test]
    public void RepairingFullCrateDoesNotChangeDurabilityOrRaiseEvent()
    {
        Crate testCrate = new Crate();
        int changeCount = 0;
        int lastReportedDurability = -1;

        testCrate.DurabilityChanged += newDurability =>
        {
            changeCount++;
            lastReportedDurability = newDurability;
        };

        bool isRepaired = testCrate.Repair(2);

        Assert.That(isRepaired, Is.False);
        Assert.That(changeCount, Is.EqualTo(0));
        Assert.That(lastReportedDurability, Is.EqualTo(-1));
        Assert.That(testCrate.Durability, Is.EqualTo(5));
    }

    [Test]
    public void RepairingDamagedCrateRaisesEventWithUpdatedDurability()
    {
        Crate testCrate = new Crate();
        testCrate.TakeDamage(4); 
        int changeCount = 0;
        int lastReportedDurability = -1;

        testCrate.DurabilityChanged += newDurability =>
        {
            changeCount++;
            lastReportedDurability = newDurability;
        };

        bool isRepaired = testCrate.Repair(2);

        Assert.That(isRepaired, Is.True);
        Assert.That(changeCount, Is.EqualTo(1));
        Assert.That(lastReportedDurability, Is.EqualTo(3));
        Assert.That(testCrate.Durability, Is.EqualTo(3));

    }

    [Test]
    public void RepairingBeyondMaximumCapsDurabilityAtFive()
    {
        Crate testCrate = new Crate();
        testCrate.TakeDamage(1); 
        int changeCount = 0;
        int lastReportedDurability = -1;

        testCrate.DurabilityChanged += newDurability =>
        {
            changeCount++;
            lastReportedDurability = newDurability;
        };

        bool isRepaired = testCrate.Repair(3);

        Assert.That(isRepaired, Is.True);
        Assert.That(changeCount, Is.EqualTo(1));
        Assert.That(lastReportedDurability, Is.EqualTo(5));
        Assert.That(testCrate.Durability, Is.EqualTo(5));

    }


}