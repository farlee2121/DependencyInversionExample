namespace Managers.RecipeManagement.RecipeEventNotifiers;

public class NullRecipeEventNotifier : IRecipeEventNotifier
{
    public void Notify(RecipeEvent recipeEvent, RecipeId recipeId)
    {
    }
}