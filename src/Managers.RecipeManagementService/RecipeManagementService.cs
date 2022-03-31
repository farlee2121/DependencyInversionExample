using System.Collections.Immutable;

namespace Managers;

public interface IRecipeManagementService
{
    void CreateRecipe(UserId userId, Recipe recipe);
    IReadOnlyCollection<Recipe> ListRecipes(UserId userId);
    PublicationResult Publish(RecipeId recipeId);
    Recipe? RecipeDetails(RecipeId recipeId);
}

public class RecipeManagementService : IRecipeManagementService
{
    private readonly IRecipeEventNotifier notifier;
    private readonly IRecipeAccess recipeAccess;

    public RecipeManagementService(IRecipeEventNotifier notifier, IRecipeAccess recipeAccess)
    {
        this.notifier = notifier;
        this.recipeAccess = recipeAccess;
    }

    public PublicationResult Publish(RecipeId recipeId)
    {

        var recipe = recipeAccess.FindRecipe(recipeId);
        if (recipe == null) return new PublicationResult.UnknownRecipe(recipeId);

        notifier.Notify(RecipeEvent.Published, recipeId);

        return new PublicationResult.Success();
    }

    public void CreateRecipe(UserId userId, Recipe recipe)
    {
        recipeAccess.CreateOrUpdateRecipe(userId, recipe);
    }
    public IReadOnlyCollection<Recipe> ListRecipes(UserId userId)
    {
        return recipeAccess.ListRecipes(userId);
    }

    public Recipe? RecipeDetails(RecipeId recipeId)
    {
        return recipeAccess.FindRecipe(recipeId);
    }
}

public record UserId(string userId);


public record PublicationResult
{
    public record Success() : PublicationResult;
    public record UnknownRecipe(RecipeId recipeId): PublicationResult;
}