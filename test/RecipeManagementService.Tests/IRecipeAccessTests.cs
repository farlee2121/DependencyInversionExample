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


    [Fact]
    public void ListRecipesReturnsEmptyByDefault()
    {
        UserId userId = NewUserId();
        var actualRecipes = sut.ListRecipes(userId);

        Assert.Empty(actualRecipes);
    }

    [Fact]
    public void CreatedRecipeIsListed()
    {
        UserId userId = NewUserId();
        Recipe expectedRecipe = GenerateRecipe();
        sut.CreateOrUpdateRecipe(userId, expectedRecipe);
        var actualRecipes = sut.ListRecipes(userId);

        Assert.Equal(expectedRecipe, actualRecipes.FirstOrDefault());
    }

    [Fact]
    public void CreatedRecipeCanBeRetrievedById()
    {
        UserId userId = NewUserId();
        Recipe expectedRecipe = GenerateRecipe();
        sut.CreateOrUpdateRecipe(userId, expectedRecipe);
        var actualRecipe = sut.FindRecipe(expectedRecipe.Id);

        Assert.Equal(expectedRecipe, actualRecipe);
    }

    [Fact]
    public void RecipeUpdateDoesNotGenerateDuplicates()
    {
        UserId userId = NewUserId();
        Recipe expectedRecipe = GenerateRecipe();

        Repeat.Action(3, () => sut.CreateOrUpdateRecipe(userId, expectedRecipe));

        var actualRecipes = sut.ListRecipes(userId);

        Assert.Equal(expectedRecipe, actualRecipes.FirstOrDefault());
        Assert.Single(actualRecipes);
    }

    [Fact]
    public void UpdateRecipeUpdatesLookupAndList()
    {
        UserId userId = NewUserId();
        Recipe originalRecipe = GenerateRecipe();

        Recipe expectedRecipe = GenerateRecipe() with { Id = originalRecipe.Id };

        sut.CreateOrUpdateRecipe(userId, originalRecipe);
        sut.CreateOrUpdateRecipe(userId, expectedRecipe);

        var actualRecipes = sut.ListRecipes(userId);

        Assert.Equal(expectedRecipe, actualRecipes.FirstOrDefault());
        Assert.Single(actualRecipes);

        var actualLookupRecipe = sut.FindRecipe(originalRecipe.Id);
        Assert.Equal(expectedRecipe, actualLookupRecipe);
    }

    [Fact]
    public void UserRecipesAreSeparate()
    {
        var userIds = Repeat.Generate(3, NewUserId);

        var expectedRecipesPerUser = userIds.ToDictionary(userId => userId, userId => Repeat.Generate(3, GenerateRecipe));

        Repeat.ForEach(expectedRecipesPerUser, kvp => Repeat.ForEach(kvp.Value, r => sut.CreateOrUpdateRecipe(kvp.Key, r)));

        var actualRecipesPerUser = userIds.ToDictionary(id => id, userId => sut.ListRecipes(userId));

        Assert.Equal(expectedRecipesPerUser, actualRecipesPerUser);
    }
    private UserId NewUserId()
    {
        return new UserId(Guid.NewGuid().ToString());
    }

    private Recipe GenerateRecipe()
    {
        return new Recipe(
            RecipeId.NewId(),
            $"Title {Guid.NewGuid()}",
            new Markdown($"Instructions {Guid.NewGuid()}")
        );
    }
}
