using AtemSharp.Types;

namespace AtemSharp.Tests.State;

[TestFixture]
public class ItemCollectionTests
{
    private class TestItem
    {
        public int Id { get; init; }
        public string Data { get; set; } = string.Empty;
    }

    [Test]
    public void Populate_CreatesCorrectNumberOfElementsByInvokingFactoryWithEachIndex()
    {
        var sut = new ItemCollection<int, TestItem>(id => new TestItem { Id = id });
        sut.Populate(5);

        Assert.Multiple(() =>
        {
            Assert.That(sut.Count, Is.EqualTo(5));
            Assert.That(sut, Is.All.InstanceOf<TestItem>());
            foreach (var id in Enumerable.Range(0, 5))
            {
                Assert.That(sut[id].Id, Is.EqualTo(id));
            }
            Assert.That(sut.Select(x => x.Id), Is.EquivalentTo(Enumerable.Range(0, 5)));
        });
    }

    [Test]
    public void Populate_WithNegativeLength_Clears()
    {
        var sut = new ItemCollection<int, TestItem>(id => new TestItem { Id = id });
        sut.Populate(5);
        sut.Populate(-1);
        Assert.That(sut, Is.Empty);
    }

    [Test]
    public void Populate_WithZeroLength_Clears()
    {
        var sut = new ItemCollection<int, TestItem>(id => new TestItem { Id = id });
        sut.Populate(5);
        sut.Populate(0);
        Assert.That(sut, Is.Empty);
    }

    [Test]
    public void Populate_WithSmallerSize_RecreatesCollection()
    {
        var sut = new ItemCollection<int, TestItem>(id => new TestItem { Id = id });
        sut.Populate(5);
        sut[0].Data = "Test";

        sut.Populate(1);
        Assert.That(sut[0].Data, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Populate_WithBiggerSize_RecreatesCollection()
    {
        var sut = new ItemCollection<int, TestItem>(id => new TestItem { Id = id });
        sut.Populate(5);
        sut[0].Data = "Test";

        sut.Populate(6);
        Assert.That(sut[0].Data, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Populate_WithSameSize_DoesNotTouchElements()
    {
        var sut = new ItemCollection<int, TestItem>(id => new TestItem { Id = id });
        sut.Populate(5);
        sut[2].Data = "Test";

        sut.Populate(5);
        Assert.That(sut[2].Data, Is.EqualTo("Test"));
    }
}
