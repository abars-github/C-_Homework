using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SortingDemo
{
    public abstract class Sorter<T> where T : IComparable<T>
    {
        public void Sort(List<T> items)
        {
            var timer = Stopwatch.StartNew();
            PerformSort(items);
            timer.Stop();
            Console.WriteLine($"{GetType().Name} took {timer.ElapsedMilliseconds} ms");
        }

        protected abstract void PerformSort(List<T> items);

        protected void Swap(List<T> items, int i, int j)
        {
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }

    public class BubbleSorter<T> : Sorter<T> where T : IComparable<T>
    {
        protected override void PerformSort(List<T> items)
        {
            for (int i = 0; i < items.Count - 1; i++)
                for (int j = 0; j < items.Count - i - 1; j++)
                    if (items[j].CompareTo(items[j + 1]) > 0)
                        Swap(items, j, j + 1);
        }
    }

    public class InsertionSorter<T> : Sorter<T> where T : IComparable<T>
    {
        protected override void PerformSort(List<T> items)
        {
            for (int i = 1; i < items.Count; i++)
            {
                T key = items[i];
                int j = i - 1;
                while (j >= 0 && items[j].CompareTo(key) > 0)
                {
                    items[j + 1] = items[j];
                    j--;
                }
                items[j + 1] = key;
            }
        }
    }

    public class SelectionSorter<T> : Sorter<T> where T : IComparable<T>
    {
        protected override void PerformSort(List<T> items)
        {
            for (int i = 0; i < items.Count - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < items.Count; j++)
                    if (items[j].CompareTo(items[minIndex]) < 0)
                        minIndex = j;

                if (minIndex != i)
                    Swap(items, i, minIndex);
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var random = new Random();
            var numbers = new List<int>();
            for (int i = 0; i < 5000; i++)
                numbers.Add(random.Next(10000));

            TestSorter(new BubbleSorter<int>(), new List<int>(numbers));
            TestSorter(new InsertionSorter<int>(), new List<int>(numbers));
            TestSorter(new SelectionSorter<int>(), new List<int>(numbers));
        }

        static void TestSorter<T>(Sorter<T> sorter, List<T> items) where T : IComparable<T>
        {
            Console.WriteLine($"Testing {sorter.GetType().Name}...");
            sorter.Sort(items);
            Console.WriteLine($"First: {items[0]}, Last: {items[items.Count - 1]}");
            Console.WriteLine();
        }
    }
}