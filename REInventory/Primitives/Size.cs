namespace REInventory.Primitives;

public record Size(int Width, int Height) {
    public int Area => Width * Height;

    public Size Rotated() {
        return new Size(Height, Width);
    }
}