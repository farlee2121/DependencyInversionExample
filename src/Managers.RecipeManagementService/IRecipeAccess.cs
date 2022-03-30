namespace Managers;

public interface IRecipeAccess
{
    Recipe? FindRecipe(RecipeId id);

    void CreateOrUpdateRecipe(Recipe recipe);

    IReadOnlyCollection<Recipe> ListRecipes();
}
