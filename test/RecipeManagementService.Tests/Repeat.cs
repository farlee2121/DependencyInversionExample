using System;

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
}
