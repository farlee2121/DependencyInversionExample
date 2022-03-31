namespace Managers.RecipeManagement;

public class NullRecipeEventNotifier : IRecipeEventNotifier
{
    public void Notify(RecipeEvent recipeEvent, RecipeId recipeId)
    {
    }
}