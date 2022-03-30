using System.Collections.Immutable;

namespace Managers;

public class RecipeManagementService
{
    private readonly IRecipeEventNotifier notifier;
    private readonly IRecipeAccess recipeAccess;

    public RecipeManagementService(IRecipeEventNotifier notifier, IRecipeAccess recipeAccess)
    {
        this.notifier = notifier;
        this.recipeAccess = recipeAccess;
    }
    
    public PublicationResult Publish(RecipeId recipeId){

        var recipe = recipeAccess.FindRecipe(recipeId);
        if (recipe == null) return new PublicationResult.UnknownRecipe(recipeId);

        notifier.Notify(RecipeEvent.Published, recipeId);

        return new PublicationResult.Success();
    }

    public void CreateRecipe(Recipe recipe)
    {
        recipeAccess.CreateOrUpdateRecipe(recipe);
    }
    public IReadOnlyCollection<Recipe> ListRecipes()
    {
        return recipeAccess.ListRecipes();
    }

    public Recipe? RecipeDetails(RecipeId recipeId)
    {
        return recipeAccess.FindRecipe(recipeId);
    }
}


public record PublicationResult
{
    public record Success() : PublicationResult;
    public record UnknownRecipe(RecipeId recipeId): PublicationResult;
}