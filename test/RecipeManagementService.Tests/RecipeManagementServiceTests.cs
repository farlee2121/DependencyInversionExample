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
            "Sample",
            new Markdown("Instructions")
        );
    }
    [Fact]
    public void PublishRaisesNotification()
    {
        Recipe recipe = GenerateRecipe();
        recipeAccessDouble.CreateRecipe(recipe);

        sut.Publish(recipe.Id);

        var expectedEvent = new SpyRecipeEventNotifier.Notification(RecipeEvent.Published, recipe.Id);
        Assert.Equal(expectedEvent, spyNotifier.RecievedNotifications.FirstOrDefault());
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

class InMemoryRecipeAccess : IRecipeAccess
{
    private List<Recipe> _recipes = new List<Recipe>();
    public Recipe? FindRecipe(RecipeId id)
    {
        return _recipes.FirstOrDefault(r => r.Id == id);
    }

    public void CreateRecipe(Recipe recipe)
    {
        _recipes.Add(recipe);
    }
}