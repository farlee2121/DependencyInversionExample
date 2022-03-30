using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace RecipeManagementServiceTests;

public static class Repeat
{
    public static void Action(int repetitions, Action action)
    {
        for (int i = 0; i < repetitions; i++)
        {
            action();
        }
    }

    public static IReadOnlyCollection<T> Generate<T>(int count, Func<T> generator)
    {
        return Enumerable.Range(0, count).Select(_ => generator()).ToImmutableList();
    }

    public static void ForEach<T>(IEnumerable<T> list, Action<T> action, int maxIterations = 100)
    {
        int iterations = 0;
        var enumerator = list.GetEnumerator();

        while (iterations < maxIterations && enumerator.MoveNext())
        {
            action(enumerator.Current);
        }
    }
}


