namespace REInventory.Primitives;

public record Interval(int Start, int End) {
    public bool OverlapsWith(Interval other) {
        return Start < other.End && End > other.Start;
    }

    public bool Contains(Interval other) {
        return Start <= other.Start && End >= other.End;
    }

    public bool ContainsWithExcludedEnd(int x) {
        return Start <= x && End > x;
    }
}