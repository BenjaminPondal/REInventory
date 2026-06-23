using REInventory.Primitives;

namespace REInventory;

[TestFixture]
public class PrimitivesTests {
    public class IntervalMath : PrimitivesTests {
        [Test]
        public void Intervals_Overlapping_ReturnsTrue() {
            var a = new Interval(0, 4);
            var b = new Interval(2, 6);

            Assert.That(a.OverlapsWith(b), Is.True);
            Assert.That(b.OverlapsWith(a), Is.True);
        }

        [Test]
        public void Intervals_CompletelySeparate_ReturnsFalse() {
            var a = new Interval(0, 2);
            var b = new Interval(4, 6);

            Assert.That(a.OverlapsWith(b), Is.False);
            Assert.That(b.OverlapsWith(a), Is.False);
        }

        [Test]
        public void Intervals_JustTouchingAtEdges_ReturnsFalse() {
            var a = new Interval(0, 2);
            var b = new Interval(2, 4);

            Assert.That(a.OverlapsWith(b), Is.False, "Touching intervals should not register as an overlap.");
        }

        [Test]
        public void Interval_ContainingSmallerInterval_ReturnsTrue() {
            var parent = new Interval(0, 5);
            var child = new Interval(1, 4);

            Assert.That(parent.Contains(child), Is.True);
        }

        [Test]
        public void Interval_CheckingIdenticalInterval_ContainsReturnsTrue() {
            var a = new Interval(0, 3);
            var b = new Interval(0, 3);

            Assert.That(a.Contains(b), Is.True);
        }

        [Test]
        public void Interval_ExceedingBounds_ContainsReturnsFalse() {
            var parent = new Interval(1, 4);
            var stickingOutLeft = new Interval(0, 3);
            var stickingOutRight = new Interval(2, 5);

            Assert.That(parent.Contains(stickingOutLeft), Is.False);
            Assert.That(parent.Contains(stickingOutRight), Is.False);
        }
    }

    public class BoxMath : PrimitivesTests {
        [Test]
        public void Boxes_Intersecting_OverlapsWithReturnsTrue() {
            // A 2x2 box at origin
            var boxA = new Box(Position.Zero, new Size(2, 2));
            // A 2x2 box overlapping at cell (1,1)
            var boxB = new Box(new Position(1, 1), new Size(2, 2));

            Assert.That(boxA.OverlapsWith(boxB), Is.True);
        }

        [Test]
        public void Boxes_SharingAnEdgeButNotOverlapping_ReturnsFalse() {
            // Box from x=0 to x=2
            var boxA = new Box(Position.Zero, new Size(2, 2));
            // Box starting exactly at x=2
            var boxB = new Box(new Position(2, 0), new Size(2, 2));

            Assert.That(boxA.OverlapsWith(boxB), Is.False, "Boxes next to each other should not overlap.");
        }
    }

    public class SizeMath : PrimitivesTests {
        [Test]
        public void Rotated_SwapsWidthAndHeight() {
            var shotgunSize = new Size(5, 2);

            var rotatedSize = shotgunSize.Rotated();

            Assert.That(rotatedSize.Width, Is.EqualTo(2));
            Assert.That(rotatedSize.Height, Is.EqualTo(5));
        }

        [Test]
        public void Rotated_Twice_ReturnsToOriginalSize() {
            var greenHerb = new Size(1, 2);

            var rotatedTwice = greenHerb.Rotated().Rotated();

            Assert.That(rotatedTwice, Is.EqualTo(greenHerb));
        }
    }
}