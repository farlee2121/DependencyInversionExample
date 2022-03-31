using Xunit;
using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using Managers.RecipeManagement;

namespace RecipeManagementServiceTests;

public class RecipeManagementServiceTests
{
    IRecipeManagementService sut;
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

    
    [Fact]
    public void PublishRaisesNotification()
    {
        Recipe recipe = GenerateRecipe();
        UserId userId = NewUserId();
        sut.CreateRecipe(userId, recipe);

        sut.Publish(recipe.Id);

        var expectedEvent = new SpyRecipeEventNotifier.Notification(RecipeEvent.Published, recipe.Id);
        Assert.Equal(expectedEvent, spyNotifier.RecievedNotifications.FirstOrDefault());
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
        Recipe expectedRecipe = GenerateRecipe();
        UserId userId = NewUserId();

        sut.CreateRecipe(userId, expectedRecipe);
        var actualRecipes = sut.ListRecipes(userId);

        Assert.Equal(expectedRecipe, actualRecipes.FirstOrDefault());
    }

    [Fact]
    public void UserRecipesAreSeparate()
    {
        var userIds = Repeat.Generate(3, NewUserId);

        var expectedRecipesPerUser = userIds.ToDictionary(userId => userId, userId => Repeat.Generate(3, GenerateRecipe));

        Repeat.ForEach(expectedRecipesPerUser, kvp => Repeat.ForEach(kvp.Value, r => sut.CreateRecipe(kvp.Key, r)));

        var actualRecipesPerUser = userIds.ToDictionary(id => id, userId => sut.ListRecipes(userId));

        Assert.Equal(expectedRecipesPerUser, actualRecipesPerUser);
    }

    [Fact]
    public void CreatedRecipeCanBeFoundById()
    {
        Recipe expectedRecipe = GenerateRecipe();
        UserId userId = NewUserId();

        sut.CreateRecipe(userId, expectedRecipe);
        var actualRecipe = sut.RecipeDetails(expectedRecipe.Id);

        Assert.Equal(expectedRecipe, actualRecipe);
    }

    [Fact]
    public void NonExistentRecipeDetailsAreNull()
    {
        var actualRecipe = sut.RecipeDetails(RecipeId.NewId());

        Assert.Null(actualRecipe);
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

class SpyRecipeEventNotifier : IRecipeEventNotifier
{
    public record Notification(RecipeEvent eventType, RecipeId recipeId);

    public List<Notification> RecievedNotifications { get; init; } = new List<Notification>();
    

    public void Notify(RecipeEvent recipeEvent, RecipeId recipeId)
    {
        RecievedNotifications.Add(new Notification(recipeEvent, recipeId));
    }
}
