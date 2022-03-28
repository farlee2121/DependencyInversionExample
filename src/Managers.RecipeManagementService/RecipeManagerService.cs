namespace Managers.RecipeManagementService;
  
public class RecipeManagerService
{
    private readonly IRecipeEventNotifier notifier;

    public RecipeManagerService(IRecipeEventNotifier notifier)
    {
        this.notifier = notifier;
    }
    
    public void Publish(RecipeId recipeId){
        notifier.Notify(RecipeEvent.Published, recipeId);
    }
}

public record RecipeId(Guid id);

public enum RecipeEvent{
    Published,
    Unpublished
}

public interface IRecipeEventNotifier{
    void Notify(RecipeEvent recipeEvent, RecipeId recipeId);
}