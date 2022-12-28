using Collections;

namespace ConsoleApplication;

class Program
{
    public static void Main(string[] args)
    {
        var c = new DynamicArray<int>();

        c.CollectionChanged += (_, action) =>
            Console.WriteLine(action.Action);

        foreach (var i in Enumerable.Range(1, 10))
            c.Add(i);

        Console.WriteLine("Array after filling:");
        c.Print();

        Console.WriteLine($"Count is {c.Count}.");

        c.Remove(1);
        Console.WriteLine("Array after removing '1':");
        c.Print();

        c.RemoveAt(2);
        Console.WriteLine("Array after removing at index 2:");
        c.Print();

        c.Insert(4, 99);
        Console.WriteLine("Array after inserting '99' at index 4:");
        c.Print();

        Console.WriteLine($"Elem at 4 is {c[4]}");

        Console.WriteLine("assignment: elem at 4=999");
        c[4] = 999;
        c.Print();

        Thread.Sleep(Timeout.Infinite);
    }
}

static class EnumerableExtensions
{
    public static void Print<T>(this IEnumerable<T> enumerable) =>
        Console.WriteLine($"[{string.Join(", ", enumerable.Select(t => t?.ToString() ?? "null"))}]\n");
}