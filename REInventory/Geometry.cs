namespace REInventory;

public record Size(int Width, int Height) {
    public int Area => Width * Height;
}

public record Position(int X, int Y);

public record Interval(int Start, int End) {
    public bool OverlapsWith(Interval other) {
        return Start < other.End && End > other.Start;
    }
}

public record Box(Position Offset, Size Size) {
    private Interval HorizontalInterval => new(Offset.X, Offset.X + Size.Width);
    private Interval VerticalInterval => new(Offset.Y, Offset.Y + Size.Height);

    public bool OverlapsWith(Box box) {
        return HorizontalInterval.OverlapsWith(box.HorizontalInterval) &&
               VerticalInterval.OverlapsWith(box.VerticalInterval);
    }
}