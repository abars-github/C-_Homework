class Context
{
    Strategy strategy;
    int[] array = { 12, 5, 23, 1, 30, 8, 17, 4, 26, 10, 15, 3, 22, 7, 28, 2, 14, 21, 6, 18, 13, 27, 9, 24, 11, 20, 19, 16, 25, 29 };
    
    public Context(Strategy strategy)
    {
        this.strategy = strategy;
    }
    
    public void Sort()
    {
        Console.WriteLine(strategy.GetType().Name);
        strategy.Sort(ref array);
    }

    public void Time()
    {
        int[] temporaryArray = (int[])array.Clone();
        DateTime start = DateTime.Now;
        strategy.Sort(ref temporaryArray);
        DateTime end = DateTime.Now;
        Console.WriteLine("Time = " + (end - start).TotalMilliseconds + "ms");
    }
    
    public void PrintArray()
    {
        for (int i = 0; i < array.Length; i++)
            Console.Write(array[i] + " ");
        Console.WriteLine();
    }
}

abstract class Strategy
{
    public abstract void Sort(ref int[] array);
}

class SelectionSort : Strategy
{
    public override void Sort(ref int[] array)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            int k = i;
            for (int j = i + 1; j < array.Length; j++)
                if (array[k] > array[j])
                    k = j;
            if (k != i)
            {
                int temporary = array[k];
                array[k] = array[i];
                array[i] = temporary;
            }
        }
    }
}

class InsertionSort : Strategy
{
    public override void Sort(ref int[] array)
    {
        for (int i = 1; i < array.Length; i++)
        {
            int j = 0;
            int buffer = array[i];
            for (j = i - 1; j >= 0; j--)
            {
                if (array[j] < buffer)
                    break;
                array[j + 1] = array[j];
            }
            array[j + 1] = buffer;
        }
    }
}

class MergeSort : Strategy
{
    public override void Sort(ref int[] array)
    {
        array = MergeSortAlgorithm(array);
    }

    private int[] MergeSortAlgorithm(int[] array)
    {
        if (array.Length <= 1)
            return array;

        int mid = array.Length / 2;
        int[] left = new int[mid];
        int[] right = new int[array.Length - mid];
        
        for (int i = 0; i < mid; i++)
            left[i] = array[i];

        for (int j = 0; j < right.Length; j++)  // Fixed condition
            right[j] = array[mid + j];

        left = MergeSortAlgorithm(left);
        right = MergeSortAlgorithm(right);
        return Merge(left, right);
    }

    private int[] Merge(int[] left, int[] right)
    {
        int[] result = new int[left.Length + right.Length];
        int i = 0, j = 0, k = 0;
        while (i < left.Length && j < right.Length)
        {
            if (left[i] < right[j])
                result[k++] = left[i++];
            else
                result[k++] = right[j++];
        }
        while (i < left.Length)
            result[k++] = left[i++];
        while (j < right.Length)
            result[k++] = right[j++];
        return result;
    }
}

class ShellSort : Strategy
{
    public override void Sort(ref int[] array)
    {
        int n = array.Length;
        for (int interval = n / 2; interval > 0; interval = interval / 2)
        {
            for (int i = interval; i < n; i++)
            {
                int temporary = array[i];
                int j;
                for (j = i; j >= interval && array[j - interval] > temporary; j -= interval)
                    array[j] = array[j - interval];
                array[j] = temporary;
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Context context;

        context = new Context(new SelectionSort());
        context.Sort();
        context.Time();
        context.PrintArray();

        context = new Context(new InsertionSort());
        context.Sort();
        context.Time();
        context.PrintArray();

        context = new Context(new MergeSort());
        context.Sort();
        context.Time();
        context.PrintArray();

        context = new Context(new ShellSort());
        context.Sort();
        context.Time();
        context.PrintArray();
    }
}