using Managers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Managers.RecipeManagement;

public class InMemoryRecipeAccess : IRecipeAccess
{
    private Dictionary<(UserId uId, RecipeId rId), Recipe> _recipes = new ();
    public Recipe? FindRecipe(RecipeId id)
    {
        return _recipes.FirstOrDefault(kvp => kvp.Key.rId == id).Value;
    }

    public void CreateOrUpdateRecipe(UserId userId, Recipe recipe)
    {
        _recipes[(userId,recipe.Id)] = recipe;
    }

    public IReadOnlyCollection<Recipe> ListRecipes(UserId userId)
    {
        return _recipes.Where(kvp => kvp.Key.uId == userId)
            .Select(kvp => kvp.Value)
            .ToImmutableList();
    }
}
