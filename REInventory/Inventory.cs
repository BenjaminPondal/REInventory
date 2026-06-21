namespace REInventory;

public class Inventory(Size gridSize) {
    private readonly Dictionary<Item, Position> _items = new();

    private int TotalVolumeUsed() {
        return _items.ToList().Sum(posItemPair => posItemPair.Key.Size.Area);
    }

    private int TotalVolume() {
        return gridSize.Area;
    }

    public bool HasFreeSpace() {
        return TotalVolumeUsed() < TotalVolume();
    }

    public bool AddItem(Item item) {
        if (!HasSpaceFor(item)) return false;

        var position = new Position(0, 0);

        for (var x = 0; x < gridSize.Width; x++)
            if (!IsSlotOccupied(new Position(x, 0))) {
                position = new Position(x, 0);
                break;
            }

        _items.TryAdd(item, position);
        return true;
    }

    private bool IsSlotOccupied(Position position) {
        return _items.ContainsValue(position);
    }

    public bool HasSpaceFor(Item item) {
        return TotalVolumeUsed() + item.Size.Area <= TotalVolume();
    }

    public bool Contains(Item item) {
        return _items.ContainsKey(item);
    }

    public Position GetPositionOf(Item item) {
        return _items[item];
    }
}