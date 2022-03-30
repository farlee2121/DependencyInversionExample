namespace Managers;

public record Recipe(
    RecipeId Id,
    string Title,
    Markdown Instructions
);

public record RecipeId(Guid id)
{
    public static RecipeId NewId()
    {
        return new RecipeId(Guid.NewGuid());
    }
};

public record Markdown(string text);
