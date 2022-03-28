using Xunit;
using Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManagementServiceTests;

public class RecipeManagementServiceTests
{
    RecipeManagementService sut;
    SpyRecipeEventNotifier spyNotifier;
    InMemoryRecipeAccess recipeAccessDouble;
    public RecipeManagementServiceTests()
    {
        spyNotifier = new SpyRecipeEventNotifier();
        recipeAccessDouble = new InMemoryRecipeAccess();
        sut = new RecipeManagementService(spyNotifier, recipeAccessDouble);
    }

    [Fact]
    public void PublicationFailsIfRecipeIdIvalid()
    {
        RecipeId recipeId = new RecipeId(Guid.NewGuid());
        var expectedResult = new PublicationResult.UnknownRecipe(recipeId);

        var actualResult = sut.Publish(recipeId);

        Assert.Equal(expectedResult, actualResult);
        Assert.Empty(spyNotifier.RecievedNotifications);
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
    public void PublishRaisesNotification()
    {
        Recipe recipe = GenerateRecipe();
        sut.CreateRecipe(recipe);

        sut.Publish(recipe.Id);

        var expectedEvent = new SpyRecipeEventNotifier.Notification(RecipeEvent.Published, recipe.Id);
        Assert.Equal(expectedEvent, spyNotifier.RecievedNotifications.FirstOrDefault());
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
        sut.CreateRecipe(expectedRecipe);
        var actualRecipes = sut.ListRecipes();

        Assert.Equal(expectedRecipe, actualRecipes.FirstOrDefault());
    }
}

class SpyRecipeEventNotifier : IRecipeEventNotifier
{
    public record Notification(RecipeEvent eventType, RecipeId recipeId);

    public List<Notification> RecievedNotifications { get; init; } = new List<Notification>();
    

    public void Notify(RecipeEvent recipeEvent, RecipeId recipeId)
    {
        RecievedNotifications.Add(new Notification(recipeEvent, recipeId));
    }
}
