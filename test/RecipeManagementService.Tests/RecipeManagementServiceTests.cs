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
    public RecipeManagementServiceTests()
    {
        spyNotifier = new SpyRecipeEventNotifier();
        sut = new RecipeManagementService(spyNotifier);
    }

    [Fact]
    public void PublishRaisesNotification()
    {
        RecipeId recipeId = new RecipeId(Guid.NewGuid());

        sut.Publish(recipeId);

        var expectedEvent = new SpyRecipeEventNotifier.Notification(RecipeEvent.Published, recipeId);
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