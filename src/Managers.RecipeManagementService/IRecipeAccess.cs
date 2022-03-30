namespace Managers;

public interface IRecipeAccess
{
    Recipe? FindRecipe(RecipeId id);

    void CreateOrUpdateRecipe(UserId userId, Recipe recipe);

    IReadOnlyCollection<Recipe> ListRecipes(UserId userId);
}
