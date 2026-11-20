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

    [TestCase(0)]
    [TestCase(-2)]
    public void RepairingWithInvalidAmountDoesNotChangeDurabilityOrRaiseEvent(int amount)
    {
        Crate testCrate = new Crate();
        testCrate.TakeDamage(5);
        int changeCount = 0;
        int lastReportedDurability = -1;

        testCrate.DurabilityChanged += newDurability =>
        {
            changeCount++;
            lastReportedDurability = newDurability;
        };

        bool isRepaired = testCrate.Repair(amount);

        Assert.That(isRepaired, Is.False);
        Assert.That(changeCount, Is.EqualTo(0));
        Assert.That(lastReportedDurability, Is.EqualTo(-1));
        Assert.That(testCrate.Durability, Is.EqualTo(0));
    }


    [Test]
    public void DamageBeyondRemainingDurabilityStopsAtZero()
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

            Assert.That(changeCount, Is.EqualTo(1));
            Assert.That(lastReportedDurability, Is.EqualTo(0));
            Assert.That(testCrate.Durability, Is.EqualTo(0));

            
        }

    [Test]
    public void DamagingEmptyCrateDoesNotRaiseEvent()
    {
        Crate testCrate = new Crate();
        testCrate.TakeDamage(5);
        int changeCount = 0;
        int lastReportedDurability = -1;

        testCrate.DurabilityChanged += newDurability =>
        {
            changeCount++;
            lastReportedDurability = newDurability;
        };

        testCrate.TakeDamage(1);

        Assert.That(changeCount, Is.EqualTo(0));
        Assert.That(lastReportedDurability, Is.EqualTo(-1));
        Assert.That(testCrate.Durability, Is.EqualTo(0));
        


    }

    [TestCase(0)]
    [TestCase(-2)]

    public void InvalidDamageDoesNotChangeDurabilityOrRaiseEvent(int amount)
    {
        Crate testCrate = new Crate();
        int changeCount = 0;
        int lastReportedDurability = -1;


        testCrate.DurabilityChanged += newDurability =>
        {
            changeCount++;
            lastReportedDurability = newDurability;
        };

        testCrate.TakeDamage(amount);

        Assert.That(changeCount, Is.EqualTo(0));
        Assert.That(lastReportedDurability, Is.EqualTo(-1));
        Assert.That(testCrate.Durability, Is.EqualTo(5));
    }


}