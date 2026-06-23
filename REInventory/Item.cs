using REInventory.Primitives;

namespace REInventory;

public class Item(string name, Size size) {
    public string Name { get; } = name;
    public Size Size { get; private set; } = size;

    public void Rotate90Degrees() {
        Size = Size.Rotated();
    }
}