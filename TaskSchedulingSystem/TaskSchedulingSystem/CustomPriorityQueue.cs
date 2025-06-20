using System;
using System.Collections.Generic;

namespace TaskSchedulingSystem
{
    public class CustomPriorityQueue<T>
    {
        private readonly List<(T Item, int Priority)> _items = new List<(T, int)>();

        public void Enqueue(T item, int priority)
        {
            _items.Add((item, priority));
            _items.Sort((a, b) => a.Priority.CompareTo(b.Priority)); // Lower priority value = higher priority
        }

        public bool TryDequeue(out T item)
        {
            if (_items.Count == 0)
            {
                item = default;
                return false;
            }

            item = _items[0].Item;
            _items.RemoveAt(0);
            return true;
        }

        public int Count => _items.Count;
    }
}