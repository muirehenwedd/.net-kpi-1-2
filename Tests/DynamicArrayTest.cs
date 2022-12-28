using System.Collections;
using Collections;

namespace Tests;
//tests.
public class DynamicArrayTest
{
    public class IntsTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (var i in Enumerable.Range(0, 10))
                yield return new object[] {i};
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Theory]
    [ClassData(typeof(IntsTestData))]
    public void Count_ReturnsCorrectValue(int supposedLength)
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, supposedLength))
            dynamicArray.Add(i);

        // Act
        var count = dynamicArray.Count;

        // Assert
        Assert.Equal(supposedLength, count);
    }

    [Fact]
    public void IsReadOnly_ReturnsFalse()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        // Act
        var isReadOnly = dynamicArray.IsReadOnly;

        // Assert
        Assert.False(isReadOnly);
    }

    [Fact]
    public void Add_AddsElement()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        // Act
        dynamicArray.Add(1);

        // Assert
        var array = dynamicArray.ToArray();
        Assert.Contains(1, array);
    }

    [Fact]
    public void Remove_RemovesHead()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        // Act
        var result = dynamicArray.Remove(0);

        //Assert
        var array = dynamicArray.ToArray();

        Assert.DoesNotContain(0, array);
        Assert.True(result);
    }

    [Fact]
    public void Remove_RemovesTail()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        // Act
        var result = dynamicArray.Remove(9);

        //Assert
        var array = dynamicArray.ToArray();

        Assert.DoesNotContain(9, array);
        Assert.True(result);
    }

    [Fact]
    public void Remove_RemovesMiddleElems()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        // Act
        var result = dynamicArray.Remove(2);

        //Assert
        var array = dynamicArray.ToArray();

        Assert.DoesNotContain(2, array);
        Assert.True(result);
    }

    [Fact]
    public void Remove_ReturnsFalseIfElementWasNotFound()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        var arrayBeforeRemove = dynamicArray.ToArray();

        // Act
        var result = dynamicArray.Remove(11);

        //Assert
        var arrayAfterRemove = dynamicArray.ToArray();

        Assert.Equal(arrayBeforeRemove, arrayAfterRemove);
        Assert.False(result);
    }

    [Fact]
    public void Clear_ResetsState()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        // Act
        dynamicArray.Clear();

        // Assert
        var array = dynamicArray.ToArray();
        Assert.Empty(array);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(5, true)]
    [InlineData(10, false)]
    public void Contains(int elem, bool expectedResult)
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        // Act
        var result = dynamicArray.Contains(elem);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CopyTo_ThrowsOnArgumentNull()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        // Act
        var act = new Action(() => { dynamicArray.CopyTo(null, 0); });

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void CopyTo_ThrowsOnArrayIndexLessThanZero()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        // Act
        var act = new Action(() =>
        {
            var array = new int[1];
            dynamicArray.CopyTo(array, -1);
        });

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Fact]
    public void CopyTo_ThrowsOnArgumentException()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        // Act
        var act = new Action(() =>
        {
            var array = new int[1];
            dynamicArray.CopyTo(array, 2);
        });

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void CopyTo_CopiesFromStart()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        var array = new int[dynamicArray.Count];

        // Act
        dynamicArray.CopyTo(array, 0);

        // Assert
        Assert.True(dynamicArray.SequenceEqual(array));
    }

    [Theory]
    [ClassData(typeof(IntsTestData))]
    public void CopyTo_CopiesWithArrayIndex(int arrayIndex)
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        var array = new int[dynamicArray.Count + arrayIndex];

        // Act
        dynamicArray.CopyTo(array, arrayIndex);

        // Assert
        var list = Enumerable.Range(0, 10).ToList();

        var arrayFromList = new int[list.Count + arrayIndex];
        list.CopyTo(arrayFromList, arrayIndex);

        Assert.True(array.SequenceEqual(arrayFromList));
    }

    [Theory]
    [ClassData(typeof(IntsTestData))]
    public void IndexOf_ReturnsCorrectIndex(int elem)
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        // Act
        var index = dynamicArray.IndexOf(elem);

        // Assert
        Assert.Equal(elem, index);
    }

    [Fact]
    public void IndexOf_ReturnsFalseIfElementWasNotFound()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        // Act
        var index = dynamicArray.IndexOf(11);

        // Assert
        Assert.Equal(-1, index);
    }

    [Fact]
    public void Insert_InsertsToFirstPosition()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        var list = dynamicArray.ToList();

        // Act
        dynamicArray.Insert(0, 99);

        // Assert
        list.Insert(0, 99);
        Assert.Equal(dynamicArray.ToArray(), list.ToArray());
    }

    [Fact]
    public void Insert_InsertsToFirstPositionInEmptyArray()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        var list = dynamicArray.ToList();

        // Act
        dynamicArray.Insert(0, 99);

        // Assert
        list.Insert(0, 99);
        Assert.Equal(dynamicArray.ToArray(), list.ToArray());
    }

    [Fact]
    public void Insert_InsertsToMiddlePosition()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        var list = dynamicArray.ToList();

        // Act
        dynamicArray.Insert(6, 99);

        // Assert
        list.Insert(6, 99);
        Assert.Equal(dynamicArray.ToArray(), list.ToArray());
    }

    [Fact]
    public void Insert_InsertsToLastPosition()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        var list = dynamicArray.ToList();

        // Act
        dynamicArray.Insert(10, 99);

        // Assert
        list.Insert(10, 99);
        Assert.Equal(dynamicArray.ToArray(), list.ToArray());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(11)]
    public void Insert_ThrowsOnBadIndex(int index)
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        // Act
        var act = new Action(() => dynamicArray.Insert(index, 99));

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(11)]
    public void Indexer_ThrowsOnBadIndex(int index)
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        // Act
        var act = new Action(() =>
        {
            var elem = dynamicArray[index];
        });

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Theory]
    [ClassData(typeof(IntsTestData))]
    public void Indexer_ReturnsCorrectData(int index)
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        // Act
        var elem = dynamicArray[index];

        // Assert
        Assert.Equal(index, elem);
    }

    [Theory]
    [ClassData(typeof(IntsTestData))]
    public void Indexer_SetsData(int index)
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(99);

        // Act
        dynamicArray[index] = -1;

        // Assert
        var array = dynamicArray.ToArray();
        Assert.Equal(index, Array.IndexOf(array, -1));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(11)]
    public void RemoveAt_ThrowsOnBadIndex(int badIndex)
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        // Act
        var act = new Action(() => { dynamicArray.RemoveAt(badIndex); });

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Fact]
    public void RemoveAt_RemovesAtFirstPosition()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        var list = dynamicArray.ToList();

        // Act
        dynamicArray.RemoveAt(0);

        // Assert
        list.RemoveAt(0);
        Assert.Equal(dynamicArray.ToArray(), list.ToArray());
    }

    [Fact]
    public void RemoveAt_RemovesAtMiddlePosition()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        var list = dynamicArray.ToList();

        // Act
        dynamicArray.RemoveAt(6);

        // Assert
        list.RemoveAt(6);
        Assert.Equal(dynamicArray.ToArray(), list.ToArray());
    }

    [Fact]
    public void RemoveAt_RemovesAtLastPosition()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        var list = dynamicArray.ToList();

        // Act
        dynamicArray.RemoveAt(9);

        // Assert
        list.RemoveAt(9);
        Assert.Equal(dynamicArray.ToArray(), list.ToArray());
    }

    [Fact]
    public void GetEnumerator_EnumeratesProperly()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        // Act
        var items = dynamicArray.Select(i => i);

        // Assert
        Assert.Equal(Enumerable.Range(0, 10), items);
    }

    [Fact]
    public void GetEnumeratorT_EnumeratesProperly()
    {
        // Arrange
        var dynamicArray = new DynamicArray<int>();

        foreach (var i in Enumerable.Range(0, 10))
            dynamicArray.Add(i);

        // Act
        var items = dynamicArray.OfType<int>();

        // Assert
        Assert.Equal(items, Enumerable.Range(0, 10));
    }
}