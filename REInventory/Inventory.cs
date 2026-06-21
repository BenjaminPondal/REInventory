namespace REInventory;

public class Inventory(Size gridSize) {
    private readonly Dictionary<Item, Box> _items = [];

    private int TotalVolumeUsed() {
        return _items.ToList().Sum(posItemPair => posItemPair.Key.Size.Area);
    }

    private int TotalVolume() {
        return gridSize.Area;
    }

    private bool CanFitBoundingBox(Box box) {
        return !_items.Values.ToList().Any(otherBox => otherBox.OverlapsWith(box));
    }

    public bool HasFreeSpace() {
        return TotalVolumeUsed() < TotalVolume();
    }

    public bool AddItem(Item item) {
        if (!HasSpaceFor(item)) return false;

        var position = new Position(0, 0);

        for (var x = 0; x < gridSize.Width; x++)
        for (var y = 0; y < gridSize.Height; y++) {
            if (CanFitBoundingBox(new Box(position, item.Size))) break;
            position = new Position(x, y);
        }

        _items.TryAdd(item, new Box(position, item.Size));
        return true;
    }

    public bool HasSpaceFor(Item item) {
        return TotalVolumeUsed() + item.Size.Area <= TotalVolume();
    }

    public bool Contains(Item item) {
        return _items.ContainsKey(item);
    }

    public Position GetPositionOf(Item item) {
        return _items[item].Offset;
    }
}