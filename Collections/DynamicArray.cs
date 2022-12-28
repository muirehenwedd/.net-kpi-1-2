using System.Collections;
using System.Collections.Specialized;

namespace Collections;

public class DynamicArray<T> : IList<T>, INotifyCollectionChanged
{
    private Node<T>? _head;
    private Node<T>? _tail;

    public int Count { get; private set; }

    public bool IsReadOnly => false;

    public void Add(T data)
    {
        var node = new Node<T>(data);

        if (_head == null)
            _head = node;
        else
        {
            _tail!.Next = node;
            node.Previous = _tail;
        }

        _tail = node;
        Count++;
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, data));
    }

    public bool Remove(T data)
    {
        var current = _head;

        while (current != null)
        {
            if (current.Data!.Equals(data))
            {
                break;
            }
            current = current.Next;
        }
        if (current != null)
        {
            if (current.Next != null)
            {
                current.Next.Previous = current.Previous;
            }
            else
            {
                _tail = current.Previous;
            }

            if (current.Previous != null)
            {
                current.Previous.Next = current.Next;
            }
            else
            {
                _head = current.Next;
            }
            Count--;
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, data));
            return true;
        }
        return false;
    }

    public void Clear()
    {
        _head = null;
        _tail = null;
        Count = 0;

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool Contains(T item)
    {
        var current = _head;
        while (current != null)
        {
            if (current.Data!.Equals(item))
                return true;
            current = current.Next;
        }
        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));

        if (arrayIndex + Count > array.Length)
            throw new ArgumentException(null, nameof(arrayIndex));

        var current = _head;
        for (var i = arrayIndex; i < array.Length; i++)
        {
            array[i] = current!.Data;
            current = current.Next;
        }
    }

    public int IndexOf(T item)
    {
        var current = _head;
        var currentIndex = 0;

        while (current != null)
        {
            if (current.Data!.Equals(item))
                return currentIndex;

            current = current.Next;
            currentIndex++;
        }

        return -1;
    }

    public void Insert(int index, T item)
    {
        if (index > Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var node = new Node<T>(item);

        if (index == 0)
        {
            var temp = _head;
            node.Next = temp;
            _head = node;
            if (Count == 0)
                _tail = _head;
            else
                temp!.Previous = node;

            Count++;
        }
        else if (index == Count)
        {
            Add(item);
        }
        else
        {
            var prev = GetNodeByIndex(index - 1);

            if (prev.Next != null)
            {
                var next = prev.Next;
                next.Previous = node;
                node.Next = next;
            }

            node.Previous = prev;

            prev.Next = node;

            Count++;

            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }
    }

    public T this[int index]
    {
        get => GetNodeByIndex(index).Data;
        set
        {
            GetNodeByIndex(index).Data = value;
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
        }
    }

    public void RemoveAt(int index)
    {
        var current = GetNodeByIndex(index);

        if (current.Next != null)
        {
            current.Next.Previous = current.Previous;
        }
        else
        {
            _tail = current.Previous;
        }

        if (current.Previous != null)
        {
            current.Previous.Next = current.Next;
        }
        else
        {
            _head = current.Next;
        }
        Count--;

        CollectionChanged?.Invoke(this,
            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, current.Data));
    }

    private Node<T> GetNodeByIndex(int index)
    {
        if (index < 0 || index + 1 > Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        if ((Count - index) * 2 > Count)
        {
            var current = _head;

            for (var i = 0; i < index; i++)
                current = current!.Next;

            return current!;
        }
        else
        {
            var current = _tail;

            for (var i = Count - 1; i > index; i--)
                current = current!.Previous;

            return current!;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        var current = _head;
        while (current != null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        var current = _head;
        while (current != null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
}