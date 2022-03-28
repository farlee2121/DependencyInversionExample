using Xunit;
using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace RecipeManagementServiceTests;

public abstract class IRecipeAccessTests
{
    IRecipeAccess sut;

    public abstract IRecipeAccess SutFactory();
    public IRecipeAccessTests()
    {
        sut = SutFactory();
    }

    private Recipe GenerateRecipe()
    {
        return new Recipe(
            RecipeId.NewId(),
            $"Title {Guid.NewGuid()}",
            new Markdown($"Instructions {Guid.NewGuid()}")
        );
    }


    [Fact]
    public void ListRecipesReturnsEmptyByDefault()
    {
        var actualRecipes = sut.ListRecipes();

        Assert.Empty(actualRecipes);
    }

    [Fact]
    public void CreatedRecipeIsListed()
    {
        Recipe expectedRecipe = GenerateRecipe();
        sut.CreateOrUpdateRecipe(expectedRecipe);
        var actualRecipes = sut.ListRecipes();

        Assert.Equal(expectedRecipe, actualRecipes.FirstOrDefault());
    }

    [Fact]
    public void CreatedRecipeCanBeRetrievedById()
    {
        Recipe expectedRecipe = GenerateRecipe();
        sut.CreateOrUpdateRecipe(expectedRecipe);
        var actualRecipe = sut.FindRecipe(expectedRecipe.Id);

        Assert.Equal(expectedRecipe, actualRecipe);
    }

    [Fact]
    public void RecipeUpdateDoesNotGenerateDuplicates()
    {
        Recipe expectedRecipe = GenerateRecipe();

        Repeat.Action(3, () => sut.CreateOrUpdateRecipe(expectedRecipe));

        var actualRecipes = sut.ListRecipes();

        Assert.Equal(expectedRecipe, actualRecipes.FirstOrDefault());
        Assert.Single(actualRecipes);
    }
}

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

public class InMemoryRecipeAccessTests : IRecipeAccessTests
{
    public override IRecipeAccess SutFactory()
    {
        return new InMemoryRecipeAccess();
    }
}