using System;
using System.Diagnostics;

namespace ConsoleApplication
{
    public static class Benchmark
    {
        public static void Run(Action task, int reps)
        {
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < reps; ++i)
                task.Invoke();
            watch.Stop();

            Console.WriteLine("Benchmark:");
            Console.WriteLine($"  {reps} reps / {watch.ElapsedMilliseconds} ms");
            if (watch.ElapsedMilliseconds != 0)
                Console.WriteLine($"  {(decimal)reps / (decimal)watch.ElapsedMilliseconds:0.##} reps/ms");
        }
    }
}
