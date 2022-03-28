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
}


public record PublicationResult
{
    public record Success() : PublicationResult;
    public record UnknownRecipe(RecipeId recipeId): PublicationResult;
}

public enum RecipeEvent{
    Published,
    Unpublished
}

public interface IRecipeEventNotifier{
    void Notify(RecipeEvent recipeEvent, RecipeId recipeId);
}

public record RecipeId(Guid id)
{
    public static RecipeId NewId()
    {
        return new RecipeId(Guid.NewGuid());
    }
};

public record Markdown(string text);
public record Recipe(
    RecipeId Id,
    string Title,
    Markdown Instructions
);
public interface IRecipeAccess
{
    Recipe? FindRecipe(RecipeId id);
}
