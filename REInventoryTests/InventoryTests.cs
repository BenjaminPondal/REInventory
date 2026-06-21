namespace REInventory;

public class InventoryTests {
    private Item _greenHerb;
    private Item _gunpowder;
    private Item _redHerb;
    private Item _shotgun;

    [SetUp]
    public void SetUp() {
        _greenHerb = new Item("Green Herb", new Size(1, 2));
        _redHerb = new Item("Red Herb", new Size(1, 2));
        _gunpowder = new Item("Gunpowder", new Size(1, 1));
        _shotgun = new Item("Shotgun", new Size(5, 2));
    }

    [Test]
    public void EmptyInventory_HasFreeSpace() {
        var inventory = new Inventory(new Size(1, 1));

        Assert.That(inventory.HasFreeSpace(), Is.True);
    }

    [Test]
    public void Given1x1Inventory_Adding1x1Item_FillsInventory() {
        var inventory = new Inventory(new Size(1, 1));

        inventory.AddItem(_gunpowder);

        Assert.That(inventory.HasFreeSpace(), Is.False);
        Assert.That(inventory.HasSpaceFor(_gunpowder), Is.False);
    }

    [Test]
    public void Given1x2Inventory_Adding1x2Item_FillsInventory() {
        var inventory = new Inventory(new Size(1, 2));

        inventory.AddItem(_greenHerb);

        Assert.That(inventory.HasFreeSpace(), Is.False);
        Assert.That(inventory.HasSpaceFor(_greenHerb), Is.False);
    }

    [Test]
    public void InventoryContainsAddedItem() {
        var inventory = new Inventory(new Size(1, 1));

        inventory.AddItem(_gunpowder);

        Assert.That(inventory.Contains(_gunpowder), Is.True);
    }

    [Test]
    public void CantAddItemToFullInventory() {
        var inventory = new Inventory(new Size(1, 1));

        inventory.AddItem(_gunpowder);
        var success = inventory.AddItem(_greenHerb);

        Assert.That(success, Is.False);
        Assert.That(inventory.Contains(_greenHerb), Is.False);
    }

    [Test]
    public void CantAddItemThatIsTooBigToFit() {
        var inventory = new Inventory(new Size(2, 2));

        var success = inventory.AddItem(_shotgun);

        Assert.That(success, Is.False);
        Assert.That(inventory.Contains(_shotgun), Is.False);
    }

    [Test]
    public void GivenEmptyInventory_AddingAnItem_PlacesItAtFirstFreeSpace() {
        var inventory = new Inventory(new Size(2, 2));

        inventory.AddItem(_greenHerb);

        Assert.That(inventory.GetPositionOf(_greenHerb), Is.EqualTo(new Position(0, 0)));
    }

    [Test]
    public void GivenInventoryWithDesiredItemSpace_AddingAnItem_PlacesItAtFirstFreeSpaceLeftToRight() {
        var inventory = new Inventory(new Size(2, 2));

        inventory.AddItem(_greenHerb);
        inventory.AddItem(_redHerb);

        Assert.That(inventory.GetPositionOf(_greenHerb), Is.EqualTo(new Position(0, 0)));
        Assert.That(inventory.GetPositionOf(_redHerb), Is.EqualTo(new Position(1, 0)));
    }

    [Test]
    public void GivenInventoryWithDesiredItemSpace_AddingAnItem_PlacesItAtFirstFreeSpaceTopToBottom() {
        var inventory = new Inventory(new Size(2, 2));
        var longBox = new Item("Long Box", new Size(2, 1));

        inventory.AddItem(longBox);
        inventory.AddItem(_gunpowder);

        Assert.That(inventory.GetPositionOf(longBox), Is.EqualTo(new Position(0, 0)));
        Assert.That(inventory.GetPositionOf(_gunpowder), Is.EqualTo(new Position(0, 1)));
    }
}