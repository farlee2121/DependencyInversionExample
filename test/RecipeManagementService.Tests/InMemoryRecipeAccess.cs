using Managers;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace RecipeManagementServiceTests;

class InMemoryRecipeAccess : IRecipeAccess
{
    private Dictionary<RecipeId, Recipe> _recipes = new ();
    public Recipe? FindRecipe(RecipeId id)
    {
        return _recipes.GetValueOrDefault(id);
    }

    public void CreateOrUpdateRecipe(Recipe recipe)
    {
        _recipes[recipe.Id] = recipe;
    }

    public IReadOnlyCollection<Recipe> ListRecipes()
    {
        return _recipes.Values.ToImmutableList();
    }
}