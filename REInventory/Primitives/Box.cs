namespace REInventory.Primitives;

public record Box(Position Offset, Size Size) {
    private Interval HorizontalSpan => new(Offset.X, Offset.X + Size.Width);
    private Interval VerticalSpan => new(Offset.Y, Offset.Y + Size.Height);

    public bool OverlapsWith(Box other) {
        return HorizontalSpan.OverlapsWith(other.HorizontalSpan) &&
               VerticalSpan.OverlapsWith(other.VerticalSpan);
    }

    public bool Contains(Box other) {
        return HorizontalSpan.Contains(other.HorizontalSpan) && VerticalSpan.Contains(other.VerticalSpan);
    }
}