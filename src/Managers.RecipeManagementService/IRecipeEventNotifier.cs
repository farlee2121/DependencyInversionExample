namespace Managers;


public enum RecipeEvent
{
    Published,
    Unpublished
}
public interface IRecipeEventNotifier{
    void Notify(RecipeEvent recipeEvent, RecipeId recipeId);
}
