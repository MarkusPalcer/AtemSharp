using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Tests.State;

[TestFixture]
public class AtemStateUtilTests
{
    private class TestArrayItem : ArrayItem
    {
        internal override void SetId(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }

    [Test]
    public void CreateArray_CreatesCorrectNumberOfElements()
    {
        var result = AtemStateUtil.CreateArray<TestArrayItem>(5);
        Assert.That(result.Count, Is.EqualTo(5));

        Assert.That(result, Is.All.InstanceOf<TestArrayItem>());
    }

    [Test]
    public void CreatArray_SetsCorrectIds()
    {
        var result = AtemStateUtil.CreateArray<TestArrayItem>(5);

        Assert.Multiple(() =>
        {
            for (var i = 0; i < 5; i++)
            {
                Assert.That(result.ElementAt(i).Id, Is.EqualTo(i));
            }
        });
    }

    [Test]
    public void CreateArray_WithNegativeLength_ReturnsEmptyArray()
    {
        var result = AtemStateUtil.CreateArray<TestArrayItem>(-1);
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public void CreateArray_WithZeroLength_ReturnsEmptyArray()
    {
        var result = AtemStateUtil.CreateArray<TestArrayItem>(0);
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public void ExpandToFit_IfListBigEnough_DoesNotChangeAnything()
    {
        // Arrange: Fill the whole list with the same item
        var item = new TestArrayItem { Id = -1 };
        var target = new List<TestArrayItem>(Enumerable.Repeat(item, 5));

        // Act
        target.ExpandToFit(2);

        // Assert
        Assert.That(target.Count, Is.EqualTo(5));
        Assert.That(target, Is.All.SameAs(item));
        Assert.That(item.Id, Is.EqualTo(-1));
    }

    [Test]
    public void ExpandToFit_IfListIsSameSize_DoesNotChangeAnything()
    {
        var item = new TestArrayItem { Id = -1 };

        var target = new List<TestArrayItem>(Enumerable.Repeat(item, 5));

        // Act
        target.ExpandToFit(4);

        // Assert
        Assert.That(target.Count, Is.EqualTo(5));
        Assert.That(target, Is.All.SameAs(item));
        Assert.That(item.Id, Is.EqualTo(-1));
    }

    [Test]
    public void ExpandToFit_IfIndexIsBiggerThanList_AddsItems()
    {
        var item = new TestArrayItem { Id = -1 };

        var target = new List<TestArrayItem>(Enumerable.Repeat(item, 5));

        // Act
        target.ExpandToFit(11);

        // Assert
        Assert.That(target.Count, Is.EqualTo(12));
        Assert.That(target.Select(x => x.Id), Is.EquivalentTo(new[]{
            -1, -1, -1, -1, -1, 5, 6,7, 8, 9, 10,11
        }));
    }

    [Test]
    public void GetOrCreate_IfIdExists_ReturnsExistingItem()
    {
        var item = new TestArrayItem { Id = 2 };
        var target = new Dictionary<int, TestArrayItem>()
        {
            {2, item},
        };

        var retrieved = target.GetOrCreate(2);

        Assert.That(retrieved, Is.SameAs(item));
    }

    [Test]
    public void GetOrCreate_IfIdDoesNotExist_CreatesNewInstance()
    {
        var item = new TestArrayItem { Id = 2 };
        var target = new Dictionary<int, TestArrayItem>
        {
            {2, item},
        };

        var retrieved = target.GetOrCreate(3);

        Assert.That(target.Keys.Order(), Is.EquivalentTo(new[] { 2, 3 }));
        Assert.That(target[2], Is.SameAs(item));
        Assert.That(target[3], Is.SameAs(retrieved));
        Assert.That(retrieved, Is.Not.SameAs(item));
    }

    [Test]
    public void GetOrCreate_IfDictionaryIsNull_Throws()
    {
        Dictionary<int, TestArrayItem> target = null!;
        Assert.Throws<ArgumentNullException>(() => target.GetOrCreate(1));
    }

    [Test]
    public void GetFairlight_IfFairlightAudioPresent_Returns()
    {
        var fairlightAudioState = new FairlightAudioState();
        var state = new AtemState
        {
            Audio = fairlightAudioState
        };

        var retrieved = state.GetFairlight();

        Assert.That(retrieved, Is.SameAs(fairlightAudioState));
    }

    [Test]
    public void GetFairlight_IfNoAudioPresent_Throws()
    {
        var state = new AtemState();

        Assert.Throws<InvalidOperationException>(() => state.GetFairlight());
    }

    [Test]
    public void GetFairlight_IfClassicAudioPresent_Throws()
    {
        var state = new AtemState
        {
            Audio = new ClassicAudioState()
        };

        Assert.Throws<InvalidOperationException>(() => state.GetFairlight());
    }

    [Test]
    public void GetClassicAudio_IfClassicAudioPresent_Returns()
    {
        var classicAudioState = new ClassicAudioState();
        var state = new AtemState
        {
            Audio = classicAudioState
        };

        var retrieved = state.GetClassicAudio();
        Assert.That(retrieved, Is.SameAs(classicAudioState));
    }

    [Test]
    public void GetClassicAudio_IfNoAudioPresent_Throws()
    {
        var state = new AtemState();
        Assert.Throws<InvalidOperationException>(() => state.GetClassicAudio());
    }

    [Test]
    public void GetClassicAudio_IfFairlightAudioPresent_Throws()
    {
        var state = new AtemState
        {
            Audio = new FairlightAudioState()
        };
        Assert.Throws<InvalidOperationException>(() => state.GetClassicAudio());
    }
}
