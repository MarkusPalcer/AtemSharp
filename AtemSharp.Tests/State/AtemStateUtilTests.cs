using AtemSharp.State;

namespace AtemSharp.Tests.State;

[TestFixture]
public class AtemStateUtilTests
{
	   /// <summary>
    /// Test class to use with the generic extension method
    /// </summary>
    private class TestObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    [Test]
    public void GetOrCreate_WithEmptyDictionary_ShouldCreateAndReturnNewInstance()
    {
        // Arrange
        var dictionary = new Dictionary<int, TestObject>();

        // Act
        var result = dictionary.GetOrCreate(5);

        // Assert
        Assert.That(result, Is.Not.Null, "Should create a new instance");
        Assert.That(dictionary.Count, Is.EqualTo(1), "Dictionary should contain one element");
        Assert.That(dictionary.ContainsKey(5), Is.True, "Dictionary should contain the key");
        Assert.That(dictionary[5], Is.SameAs(result), "Dictionary should contain the created instance");
    }

    [Test]
    public void GetOrCreate_WithExistingKey_ShouldReturnExistingInstance()
    {
        // Arrange
        var existingObject = new TestObject { Id = 42, Name = "Existing" };
        var dictionary = new Dictionary<int, TestObject> { { 10, existingObject } };

        // Act
        var result = dictionary.GetOrCreate(10);

        // Assert
        Assert.That(result, Is.SameAs(existingObject), "Should return the existing instance");
        Assert.That(result.Id, Is.EqualTo(42), "Should preserve existing properties");
        Assert.That(result.Name, Is.EqualTo("Existing"), "Should preserve existing properties");
        Assert.That(dictionary.Count, Is.EqualTo(1), "Dictionary size should remain the same");
    }

    [Test]
    public void GetOrCreate_WithNonExistentKey_ShouldCreateNewInstanceAndPreserveExisting()
    {
        // Arrange
        var existingObject = new TestObject { Id = 100, Name = "First" };
        var dictionary = new Dictionary<int, TestObject> { { 1, existingObject } };

        // Act
        var result = dictionary.GetOrCreate(3);

        // Assert
        Assert.That(result, Is.Not.Null, "Should create a new instance");
        Assert.That(result, Is.Not.SameAs(existingObject), "Should be a different instance");
        Assert.That(dictionary.Count, Is.EqualTo(2), "Dictionary should have two elements");
        Assert.That(dictionary[1], Is.SameAs(existingObject), "Should preserve existing element");
        Assert.That(dictionary[3], Is.SameAs(result), "Should contain the new element");
        
        // Verify existing object is unchanged
        Assert.That(existingObject.Id, Is.EqualTo(100), "Existing object should be unchanged");
        Assert.That(existingObject.Name, Is.EqualTo("First"), "Existing object should be unchanged");
    }

    [Test]
    public void GetOrCreate_WithZeroKey_ShouldWork()
    {
        // Arrange
        var dictionary = new Dictionary<int, TestObject>();

        // Act
        var result = dictionary.GetOrCreate(0);

        // Assert
        Assert.That(result, Is.Not.Null, "Should create instance for key 0");
        Assert.That(dictionary.ContainsKey(0), Is.True, "Dictionary should contain key 0");
        Assert.That(dictionary[0], Is.SameAs(result), "Should return the created instance");
    }

    [Test]
    public void GetOrCreate_WithNegativeKey_ShouldWork()
    {
        // Arrange
        var dictionary = new Dictionary<int, TestObject>();

        // Act
        var result = dictionary.GetOrCreate(-5);

        // Assert
        Assert.That(result, Is.Not.Null, "Should create instance for negative key");
        Assert.That(dictionary.ContainsKey(-5), Is.True, "Dictionary should contain negative key");
        Assert.That(dictionary[-5], Is.SameAs(result), "Should return the created instance");
    }

    [Test]
    public void GetOrCreate_MultipleCallsSameKey_ShouldReturnSameInstance()
    {
        // Arrange
        var dictionary = new Dictionary<int, TestObject>();

        // Act
        var first = dictionary.GetOrCreate(7);
        var second = dictionary.GetOrCreate(7);
        var third = dictionary.GetOrCreate(7);

        // Assert
        Assert.That(first, Is.SameAs(second), "First and second calls should return same instance");
        Assert.That(second, Is.SameAs(third), "Second and third calls should return same instance");
        Assert.That(dictionary.Count, Is.EqualTo(1), "Dictionary should only contain one element");
    }

    [Test]
    public void GetOrCreate_WithMixEffectType_ShouldCreateCorrectObject()
    {
        // Arrange
        var dictionary = new Dictionary<int, MixEffect>();

        // Act
        var result = dictionary.GetOrCreate(2);

        // Assert
        Assert.That(result, Is.Not.Null, "Should create MixEffect instance");
        Assert.That(result, Is.TypeOf<MixEffect>(), "Should be correct type");
        Assert.That(dictionary[2], Is.SameAs(result), "Should store in dictionary");
        
        // Verify default MixEffect values
        Assert.That(result.Index, Is.EqualTo(0), "Should have default Index value");
        Assert.That(result.ProgramInput, Is.EqualTo(0), "Should have default ProgramInput value");
        Assert.That(result.PreviewInput, Is.EqualTo(0), "Should have default PreviewInput value");
    }

    [Test]
    public void GetOrCreate_WithDownstreamKeyerType_ShouldCreateCorrectObject()
    {
        // Arrange
        var dictionary = new Dictionary<int, DownstreamKeyer>();

        // Act
        var result = dictionary.GetOrCreate(1);

        // Assert
        Assert.That(result, Is.Not.Null, "Should create DownstreamKeyer instance");
        Assert.That(result, Is.TypeOf<DownstreamKeyer>(), "Should be correct type");
        Assert.That(dictionary[1], Is.SameAs(result), "Should store in dictionary");
    }

    [Test]
    public void GetOrCreate_SparseIndices_ShouldWorkCorrectly()
    {
        // Arrange
        var dictionary = new Dictionary<int, TestObject>();

        // Act - Create objects at non-sequential indices
        var obj1 = dictionary.GetOrCreate(1);
        var obj10 = dictionary.GetOrCreate(10);
        var obj5 = dictionary.GetOrCreate(5);
        var obj100 = dictionary.GetOrCreate(100);

        // Assert
        Assert.That(dictionary.Count, Is.EqualTo(4), "Should have four objects");
        Assert.That(dictionary[1], Is.SameAs(obj1), "Should contain object at index 1");
        Assert.That(dictionary[5], Is.SameAs(obj5), "Should contain object at index 5");
        Assert.That(dictionary[10], Is.SameAs(obj10), "Should contain object at index 10");
        Assert.That(dictionary[100], Is.SameAs(obj100), "Should contain object at index 100");
        
        // Verify intermediate indices don't exist (sparse behavior)
        Assert.That(dictionary.ContainsKey(2), Is.False, "Should not contain index 2");
        Assert.That(dictionary.ContainsKey(3), Is.False, "Should not contain index 3");
        Assert.That(dictionary.ContainsKey(4), Is.False, "Should not contain index 4");
        Assert.That(dictionary.ContainsKey(11), Is.False, "Should not contain index 11");
    }

    [Test]
    public void GetOrCreate_ThreadSafety_ShouldNotCorruptDictionary()
    {
        // Arrange
        var dictionary = new Dictionary<int, TestObject>();
        var tasks = new List<Task<TestObject>>();
        const int numTasks = 10;
        const int keyToTest = 42;

        // Act - Multiple threads trying to get/create the same key
        for (int i = 0; i < numTasks; i++)
        {
            tasks.Add(Task.Run(() => dictionary.GetOrCreate(keyToTest)));
        }

        var results = Task.WhenAll(tasks).Result;

        // Assert
        Assert.That(dictionary.Count, Is.EqualTo(1), "Dictionary should only contain one element");
        Assert.That(dictionary.ContainsKey(keyToTest), Is.True, "Dictionary should contain the test key");
        
        // Note: This test may have race conditions due to Dictionary not being thread-safe,
        // but it helps verify that the method itself doesn't introduce additional corruption
        Assert.That(results.Length, Is.EqualTo(numTasks), "All tasks should complete");
        Assert.That(results.All(r => r != null), Is.True, "All results should be non-null");
    }

    [Test]
    public void GetOrCreate_WithCustomConstructorBehavior_ShouldUseDefaultConstructor()
    {
        // This test verifies that the method uses new T() which calls the parameterless constructor
        
        // Arrange
        var dictionary = new Dictionary<int, TestObject>();

        // Act
        var result = dictionary.GetOrCreate(1);

        // Assert
        Assert.That(result.Id, Is.EqualTo(0), "Should use default constructor - Id should be 0");
        Assert.That(result.Name, Is.EqualTo(string.Empty), "Should use default constructor - Name should be empty");
        Assert.That(result.CreatedAt, Is.Not.EqualTo(default(DateTime)), "CreatedAt should be set by constructor");
    }

    [Test]
    public void GetOrCreate_WithNullDictionary_ShouldThrowArgumentNullException()
    {
        // Arrange
        Dictionary<int, TestObject> dictionary = null!;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => dictionary.GetOrCreate(1));
        Assert.That(exception.ParamName, Is.EqualTo("dict"), "Exception should specify the correct parameter name");
    }
}