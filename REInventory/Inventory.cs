using REInventory.Primitives;

namespace REInventory;

public class Inventory(Size gridSize) {
    private readonly List<PlacedItem> _items = [];

    public Inventory(Size size, List<PlacedItem> items) : this(size) {
        _items = items.ToList();
    }

    public PlacedItem? Selection { get; private set; }

    private int TotalVolumeUsed() {
        return _items.Sum(placedItem => placedItem.Item.Size.Area);
    }

    private int TotalVolume() {
        return gridSize.Area;
    }

    private bool CanFitBoundingBox(Box box) {
        return new Box(Position.Zero, gridSize).Contains(box) &&
               !_items.Any(otherPlacedItem => otherPlacedItem.Box.OverlapsWith(box));
    }

    private Position? FindFreePosition(Size size) {
        for (var y = 0; y < gridSize.Height; y++)
        for (var x = 0; x < gridSize.Width; x++) {
            var currentPosition = new Position(x, y);

            if (CanFitBoundingBox(new Box(currentPosition, size)))
                return currentPosition;
        }

        return null;
    }

    public bool HasFreeSpace() {
        return TotalVolumeUsed() < TotalVolume();
    }

    public bool AddItem(Item item) {
        if (!HasSpaceFor(item)) return false;

        var placement = FindFreePosition(item.Size);

        if (placement == null) {
            item.Rotate90Degrees();
            placement = FindFreePosition(item.Size);
        }

        if (placement == null) return false;

        _items.Add(new PlacedItem(item, new Box(placement, item.Size)));
        return true;
    }

    public bool HasSpaceFor(Item item) {
        return TotalVolumeUsed() + item.Size.Area <= TotalVolume();
    }

    public bool Contains(Item item) {
        return _items.Any(placedItem => placedItem.Item == item);
    }

    public Position GetPivotOf(Item item) {
        return _items.First(placedItem => placedItem.Item == item).Box.Offset;
    }

    public List<PlacedItem> GetVisualElements() {
        return _items.ToList();
    }

    // TODO: Find better name
    public void SwitchSelectionWithItemAt(Position position) {
        var itemAtPosition = _items.FirstOrDefault(item => item.Box.Contains(position));

        if (Selection != null && itemAtPosition == null) {
            itemAtPosition = Selection.MoveTo(position);
            if (!CanFitBoundingBox(itemAtPosition.Box)) return;

            _items.Add(itemAtPosition);
            Selection = null;
            return;
        }

        if (itemAtPosition == null) return;

        _items.Remove(itemAtPosition);
        Selection = itemAtPosition;
    }

    public record PlacedItem(Item Item, Box Box) {
        public PlacedItem MoveTo(Position position) {
            return this with { Box = Box.TranslateTo(position) };
        }
    }
}