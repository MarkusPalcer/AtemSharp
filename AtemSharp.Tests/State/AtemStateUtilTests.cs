using AtemSharp.State;

namespace AtemSharp.Tests.State.Utilities;

[TestFixture]
public class AtemStateUtilTests
{
    [Test]
    public void GetMixEffect_WithNullVideoState_ShouldCreateVideoStateAndMixEffect()
    {
        // Arrange
        var state = new AtemState
        {
            Video = null // Explicitly null
        };

        // Act
        var result = AtemStateUtil.GetMixEffect(state, 0);

        // Assert
        Assert.That(state.Video, Is.Not.Null, "Video state should be created");
        Assert.That(state.Video.MixEffects, Is.Not.Null, "MixEffects array should be created");
        Assert.That(state.Video.MixEffects, Has.Length.EqualTo(1), "MixEffects array should have one element");
        Assert.That(state.Video.MixEffects[0], Is.Not.Null, "MixEffect should be created");
        Assert.That(result, Is.SameAs(state.Video.MixEffects[0]), "Should return the created MixEffect");
    }

    [Test]
    public void GetMixEffect_WithEmptyMixEffectsArray_ShouldCreateMixEffect()
    {
        // Arrange
        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = [] // Empty array
            }
        };

        // Act
        var result = AtemStateUtil.GetMixEffect(state, 0);

        // Assert
        Assert.That(state.Video.MixEffects, Has.Length.EqualTo(1), "MixEffects array should be resized");
        Assert.That(state.Video.MixEffects[0], Is.Not.Null, "MixEffect should be created");
        Assert.That(result, Is.SameAs(state.Video.MixEffects[0]), "Should return the created MixEffect");
    }

    [Test]
    public void GetMixEffect_WithExistingMixEffect_ShouldReturnExisting()
    {
        // Arrange
        var existingMixEffect = new MixEffect
        {
            Index = 0,
            ProgramInput = 1234,
            PreviewInput = 5678
        };

        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = [existingMixEffect]
            }
        };

        // Act
        var result = AtemStateUtil.GetMixEffect(state, 0);

        // Assert
        Assert.That(result, Is.SameAs(existingMixEffect), "Should return the existing MixEffect");
        Assert.That(result.ProgramInput, Is.EqualTo(1234), "Should preserve existing properties");
        Assert.That(result.PreviewInput, Is.EqualTo(5678), "Should preserve existing properties");
    }

    [Test]
    public void GetMixEffect_WithInsufficientArraySize_ShouldExpandArray()
    {
        // Arrange
        var existingMixEffect = new MixEffect { Index = 0 };
        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = [existingMixEffect] // Array of size 1
            }
        };

        // Act
        var result = AtemStateUtil.GetMixEffect(state, 2); // Request index 2

        // Assert
        Assert.That(state.Video.MixEffects, Has.Length.EqualTo(3), "Array should be expanded to size 3");
        Assert.That(state.Video.MixEffects[0], Is.SameAs(existingMixEffect), "Existing element should be preserved");
        Assert.That(state.Video.MixEffects[1], Is.Null, "Intermediate element should be null");
        Assert.That(state.Video.MixEffects[2], Is.Not.Null, "Requested element should be created");
        Assert.That(result, Is.SameAs(state.Video.MixEffects[2]), "Should return the created MixEffect");
    }

    [Test]
    public void GetMixEffect_WithNullElementInArray_ShouldCreateMixEffect()
    {
        // Arrange
        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = new MixEffect?[3] { null, null, null }
            }
        };

        // Act
        var result = AtemStateUtil.GetMixEffect(state, 1);

        // Assert
        Assert.That(state.Video.MixEffects, Has.Length.EqualTo(3), "Array size should remain the same");
        Assert.That(state.Video.MixEffects[0], Is.Null, "Other elements should remain null");
        Assert.That(state.Video.MixEffects[1], Is.Not.Null, "Requested element should be created");
        Assert.That(state.Video.MixEffects[2], Is.Null, "Other elements should remain null");
        Assert.That(result, Is.SameAs(state.Video.MixEffects[1]), "Should return the created MixEffect");
    }

    [Test]
    public void GetMixEffect_CreatedMixEffect_ShouldHaveCorrectDefaultValues()
    {
        // Arrange
        var state = new AtemState();

        // Act
        var result = AtemStateUtil.GetMixEffect(state, 0);

        // Assert
        Assert.That(result.Index, Is.EqualTo(0), "Index should be set correctly");
        Assert.That(result.ProgramInput, Is.EqualTo(0), "ProgramInput should have default value");
        Assert.That(result.PreviewInput, Is.EqualTo(0), "PreviewInput should have default value");
        Assert.That(result.TransitionPreview, Is.False, "TransitionPreview should be false");
        
        Assert.That(result.TransitionPosition, Is.Not.Null, "TransitionPosition should be created");
        Assert.That(result.TransitionPosition.InTransition, Is.False, "TransitionPosition.InTransition should be false");
        Assert.That(result.TransitionPosition.HandlePosition, Is.EqualTo(0), "TransitionPosition.HandlePosition should be 0");
        Assert.That(result.TransitionPosition.RemainingFrames, Is.EqualTo(0), "TransitionPosition.RemainingFrames should be 0");
        
        Assert.That(result.TransitionProperties, Is.Not.Null, "TransitionProperties should be created");
        Assert.That(result.TransitionSettings, Is.Not.Null, "TransitionSettings should be created");
        Assert.That(result.UpstreamKeyers, Is.Not.Null, "UpstreamKeyers should be created");
        Assert.That(result.UpstreamKeyers, Is.Empty, "UpstreamKeyers should be empty array");
    }

    [Test]
    public void GetMixEffect_WithDifferentIndices_ShouldCreateCorrectIndices()
    {
        // Arrange
        var state = new AtemState();

        // Act
        var mixEffect0 = AtemStateUtil.GetMixEffect(state, 0);
        var mixEffect2 = AtemStateUtil.GetMixEffect(state, 2);
        var mixEffect1 = AtemStateUtil.GetMixEffect(state, 1);

        // Assert
        Assert.That(mixEffect0.Index, Is.EqualTo(0), "MixEffect 0 should have correct index");
        Assert.That(mixEffect1.Index, Is.EqualTo(1), "MixEffect 1 should have correct index");
        Assert.That(mixEffect2.Index, Is.EqualTo(2), "MixEffect 2 should have correct index");
        
        // Verify they're stored in the correct array positions
        Assert.That(state.Video!.MixEffects[0], Is.SameAs(mixEffect0), "MixEffect 0 should be at array index 0");
        Assert.That(state.Video.MixEffects[1], Is.SameAs(mixEffect1), "MixEffect 1 should be at array index 1");
        Assert.That(state.Video.MixEffects[2], Is.SameAs(mixEffect2), "MixEffect 2 should be at array index 2");
    }

    [Test]
    public void GetMixEffect_MultipleCallsSameIndex_ShouldReturnSameInstance()
    {
        // Arrange
        var state = new AtemState();

        // Act
        var firstCall = AtemStateUtil.GetMixEffect(state, 1);
        var secondCall = AtemStateUtil.GetMixEffect(state, 1);
        var thirdCall = AtemStateUtil.GetMixEffect(state, 1);

        // Assert
        Assert.That(firstCall, Is.SameAs(secondCall), "Second call should return same instance");
        Assert.That(secondCall, Is.SameAs(thirdCall), "Third call should return same instance");
        Assert.That(state.Video!.MixEffects, Has.Length.EqualTo(2), "Array should only be expanded once");
    }

    [Test]
    public void GetMixEffect_WithLargeIndex_ShouldExpandArrayCorrectly()
    {
        // Arrange
        var state = new AtemState();

        // Act
        var result = AtemStateUtil.GetMixEffect(state, 10);

        // Assert
        Assert.That(state.Video!.MixEffects, Has.Length.EqualTo(11), "Array should be expanded to accommodate index 10");
        Assert.That(state.Video.MixEffects[10], Is.SameAs(result), "MixEffect should be at the correct index");
        Assert.That(result.Index, Is.EqualTo(10), "MixEffect should have correct index");
        
        // Verify all other elements are null
        for (int i = 0; i < 10; i++)
        {
            Assert.That(state.Video.MixEffects[i], Is.Null, $"Element {i} should be null");
        }
    }

    [Test]
    public void GetMixEffect_WithExistingVideoStateAndOtherProperties_ShouldPreserveOtherProperties()
    {
        // Arrange
        var existingDownstreamKeyer = new DownstreamKeyer();
        var state = new AtemState
        {
            Video = new VideoState
            {
                DownstreamKeyers = [existingDownstreamKeyer],
                MixEffects = []
            }
        };

        // Act
        var result = AtemStateUtil.GetMixEffect(state, 0);

        // Assert
        Assert.That(state.Video.DownstreamKeyers, Has.Length.EqualTo(1), "DownstreamKeyers should be preserved");
        Assert.That(state.Video.DownstreamKeyers[0], Is.SameAs(existingDownstreamKeyer), "Existing DownstreamKeyer should be preserved");
        Assert.That(state.Video.MixEffects, Has.Length.EqualTo(1), "MixEffects should be created");
        Assert.That(result, Is.Not.Null, "MixEffect should be created");
    }

    [Test]
    public void GetMixEffect_WithZeroIndex_ShouldWork()
    {
        // Arrange
        var state = new AtemState();

        // Act
        var result = AtemStateUtil.GetMixEffect(state, 0);

        // Assert
        Assert.That(result, Is.Not.Null, "Should create MixEffect for index 0");
        Assert.That(result.Index, Is.EqualTo(0), "Index should be 0");
        Assert.That(state.Video!.MixEffects, Has.Length.EqualTo(1), "Array should have one element");
    }

    [Test]
    public void GetMixEffect_ArrayCopy_ShouldPreserveExistingElements()
    {
        // Arrange
        var mixEffect0 = new MixEffect { Index = 0, ProgramInput = 100 };
        var mixEffect1 = new MixEffect { Index = 1, ProgramInput = 200 };
        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = [mixEffect0, mixEffect1]
            }
        };

        // Act - Request a higher index to trigger array expansion
        var result = AtemStateUtil.GetMixEffect(state, 4);

        // Assert
        Assert.That(state.Video.MixEffects, Has.Length.EqualTo(5), "Array should be expanded");
        Assert.That(state.Video.MixEffects[0], Is.SameAs(mixEffect0), "Original element 0 should be preserved");
        Assert.That(state.Video.MixEffects[1], Is.SameAs(mixEffect1), "Original element 1 should be preserved");
        Assert.That(state.Video.MixEffects[2], Is.Null, "New element 2 should be null");
        Assert.That(state.Video.MixEffects[3], Is.Null, "New element 3 should be null");
        Assert.That(state.Video.MixEffects[4], Is.SameAs(result), "New element 4 should be the created MixEffect");
        
        // Verify original elements maintain their properties
        Assert.That(mixEffect0.ProgramInput, Is.EqualTo(100), "Original element properties should be preserved");
        Assert.That(mixEffect1.ProgramInput, Is.EqualTo(200), "Original element properties should be preserved");
    }
}