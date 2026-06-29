using REInventory.Primitives;

namespace REInventory;

public class InventoryTests {
    private List<Inventory.PlacedItem> _collectionOfPlacedItems;
    private Inventory _emptyOneByOneInventory;
    private Inventory _emptyTwoByTwoInventory;
    private Item _greenHerb;
    private Item _gunpowder;
    private Inventory _inventoryWithItems;
    private Inventory.PlacedItem _placedGreenHerb;
    private Inventory.PlacedItem _placedGunpowder;
    private Item _redHerb;
    private Item _shotgun;

    [SetUp]
    public void SetUp() {
        _greenHerb = new Item("Green Herb", new Size(1, 2));
        _redHerb = new Item("Red Herb", new Size(1, 2));
        _gunpowder = new Item("Gunpowder", new Size(1, 1));
        _shotgun = new Item("Shotgun", new Size(5, 2));

        _placedGunpowder = new Inventory.PlacedItem(_gunpowder, new Box(Position.Zero, _gunpowder.Size));
        _placedGreenHerb = new Inventory.PlacedItem(_greenHerb, new Box(new Position(0, 0), _greenHerb.Size));

        _collectionOfPlacedItems = [
            _placedGunpowder,
            new Inventory.PlacedItem(new Item("Coin", new Size(1, 1)), new Box(new Position(1, 1), new Size(1, 1)))
        ];

        _emptyOneByOneInventory = new Inventory(new Size(1, 1));
        _emptyTwoByTwoInventory = new Inventory(new Size(2, 2));
        _inventoryWithItems = new Inventory(new Size(2, 2), _collectionOfPlacedItems);
    }

    [Test]
    public void EmptyInventory_HasFreeSpace() {
        Assert.That(_emptyOneByOneInventory.HasFreeSpace(), Is.True);
    }

    [Test]
    public void Given1x1Inventory_Adding1x1Item_FillsInventory() {
        _emptyOneByOneInventory.AddItem(_gunpowder);

        Assert.That(_emptyOneByOneInventory.HasFreeSpace(), Is.False);
        Assert.That(_emptyOneByOneInventory.HasSpaceFor(_gunpowder), Is.False);
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
        _emptyOneByOneInventory.AddItem(_gunpowder);

        Assert.That(_emptyOneByOneInventory.Contains(_gunpowder), Is.True);
    }

    [Test]
    public void CantAddItemToFullInventory() {
        _emptyOneByOneInventory.AddItem(_gunpowder);
        var success = _emptyOneByOneInventory.AddItem(_greenHerb);

        Assert.That(success, Is.False);
        Assert.That(_emptyOneByOneInventory.Contains(_greenHerb), Is.False);
    }

    [Test]
    public void CantAddItemThatIsTooBigToFit() {
        var success = _emptyTwoByTwoInventory.AddItem(_shotgun);

        Assert.That(success, Is.False);
        Assert.That(_emptyTwoByTwoInventory.Contains(_shotgun), Is.False);
    }

    [Test]
    public void GivenEmptyInventory_AddingAnItem_PlacesItAtFirstFreeSpaceStartingAtTopLeft() {
        _emptyTwoByTwoInventory.AddItem(_greenHerb);

        Assert.That(_emptyTwoByTwoInventory.GetPivotOf(_greenHerb), Is.EqualTo(Position.Zero));
    }

    [Test]
    public void GivenInventoryWithDesiredItemSpace_AddingAnItem_PlacesItAtFirstFreeSpaceLeftToRight() {
        _emptyTwoByTwoInventory.AddItem(_greenHerb);
        _emptyTwoByTwoInventory.AddItem(_redHerb);

        Assert.That(_emptyTwoByTwoInventory.GetPivotOf(_greenHerb), Is.EqualTo(Position.Zero));
        Assert.That(_emptyTwoByTwoInventory.GetPivotOf(_redHerb), Is.EqualTo(new Position(1, 0)));
    }

    [Test]
    public void GivenInventoryWithDesiredItemSpace_AddingAnItem_PlacesItAtFirstFreeSpaceTopToBottom() {
        var longBox = new Item("Long Box", new Size(2, 1));

        _emptyTwoByTwoInventory.AddItem(longBox);
        _emptyTwoByTwoInventory.AddItem(_gunpowder);

        Assert.That(_emptyTwoByTwoInventory.GetPivotOf(longBox), Is.EqualTo(Position.Zero));
        Assert.That(_emptyTwoByTwoInventory.GetPivotOf(_gunpowder), Is.EqualTo(new Position(0, 1)));
    }

    [Test]
    public void GivenInventoryWithDesiredItemSpace_AddingAnItem_PlacesItAtFirstFreeSpaceWith90DegreesRotation() {
        var twoByOne = new Size(2, 1);
        var longBox = new Item("Long Box", twoByOne);

        _emptyTwoByTwoInventory.AddItem(longBox);
        _emptyTwoByTwoInventory.AddItem(_greenHerb);

        var boundingBoxes = _emptyTwoByTwoInventory.GetVisualElements();
        Assert.That(boundingBoxes,
            Contains.Item(new Inventory.PlacedItem(longBox, new Box(Position.Zero, twoByOne))));
        Assert.That(boundingBoxes,
            Contains.Item(new Inventory.PlacedItem(_greenHerb, new Box(new Position(0, 1), twoByOne))));
    }

    [Test]
    public void GivenInventoryWithEnoughSpaceButWrongGeometry_CantAddItem() {
        var success = _inventoryWithItems.AddItem(_greenHerb);

        Assert.That(success, Is.False);
        Assert.That(_inventoryWithItems.Contains(_greenHerb), Is.False);
    }

    [Test]
    public void SelectingSlotWithoutItem_ShouldHaveNoSelection() {
        _inventoryWithItems.SwitchSelectionWithItemAt(new Position(1, 0));

        Assert.That(_inventoryWithItems.Selection, Is.Null);
    }

    [Test]
    public void SelectingSlotWithItemPivot_ShouldHaveItemSelected() {
        _inventoryWithItems.SwitchSelectionWithItemAt(Position.Zero);

        Assert.That(_inventoryWithItems.Selection, Is.EqualTo(_placedGunpowder));
    }

    [Test]
    public void SelectingSlotInsideItemBoundingBox_ShouldHaveItemSelected() {
        var inventory = new Inventory(new Size(2, 2), [_placedGreenHerb]);

        inventory.SwitchSelectionWithItemAt(new Position(0, 1));

        Assert.That(inventory.Selection, Is.EqualTo(_placedGreenHerb));
    }

    [Test]
    public void SelectingItem_ShouldRemoveItemFromInventory() {
        _inventoryWithItems.SwitchSelectionWithItemAt(Position.Zero);

        Assert.That(_inventoryWithItems.Contains(_placedGunpowder.Item), Is.False);
    }

    [Test]
    public void GivenSelectedItem_SwitchingSelectionAtEmptySlotWithEnoughSpace_ShouldPlaceSelectedItem() {
        _inventoryWithItems.SwitchSelectionWithItemAt(Position.Zero); // Select first
        _inventoryWithItems.SwitchSelectionWithItemAt(new Position(1, 0)); // Then drop

        var newPlacedItem = new Inventory.PlacedItem(_gunpowder, new Box(new Position(1, 0), _gunpowder.Size));
        Assert.That(_inventoryWithItems.GetVisualElements(), Contains.Item(newPlacedItem));
        Assert.That(_inventoryWithItems.Selection, Is.Null);
    }

    [Test]
    public void GivenSelectedItem_SwitchingSelectionAtEmptySlotWithNotEnoughSpace_ShouldNotPlaceSelectedItem() {
        var inventory = new Inventory(new Size(2, 2),
        [
            _placedGreenHerb,
            new Inventory.PlacedItem(new Item("Long Box", new Size(2, 1)), new Box(new Position(0, 1), new Size(2, 1)))
        ]);

        inventory.SwitchSelectionWithItemAt(Position.Zero); // Select first
        inventory.SwitchSelectionWithItemAt(new Position(1, 0)); // Then drop

        Assert.That(inventory.Contains(_placedGreenHerb.Item), Is.False);
        Assert.That(inventory.Selection, Is.EqualTo(_placedGreenHerb));
    }
}