namespace REInventory.Primitives;

public record Position(int X, int Y) {
    public static readonly Position Zero = new(0, 0);
}