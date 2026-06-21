namespace REInventory;

public record Size(int Width, int Height) {
    public int Area => Width * Height;
}

public record Position(int X, int Y);