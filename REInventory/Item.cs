namespace REInventory;

public class Item(string name, Size size) {
    public string Name { get; } = name;
    public Size Size { get; } = size;
}