namespace Managers;
  
public class RecipeManagementService
{
    private readonly IRecipeEventNotifier notifier;

    public RecipeManagementService(IRecipeEventNotifier notifier)
    {
        this.notifier = notifier;
    }
    
    public void Publish(RecipeId recipeId){
        notifier.Notify(RecipeEvent.Published, recipeId);
    }
}



public enum RecipeEvent{
    Published,
    Unpublished
}

public interface IRecipeEventNotifier{
    void Notify(RecipeEvent recipeEvent, RecipeId recipeId);
}

public record RecipeId(Guid id);
