using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.RecipeManagement.Adapters.RecipeEventNotifiers
{
    public class SearchIndexRecipeEventNotifier : IRecipeEventNotifier
    {
        private readonly RecipeSearchAccessor searchAccessor;
        private readonly IRecipeAccess recipeAccess;

        public SearchIndexRecipeEventNotifier(RecipeSearchAccessor searchAccessor, IRecipeAccess recipeAccess)
        {
            this.searchAccessor = searchAccessor;
            this.recipeAccess = recipeAccess;
        }
        public void Notify(RecipeEvent recipeEvent, RecipeId recipeId)
        {
            if(recipeEvent == RecipeEvent.Published)
            {
                var recipe = recipeAccess.FindRecipe(recipeId);
                if(recipe != null) searchAccessor.IndexRecipe(recipe);
            }
            else if(recipeEvent == RecipeEvent.Unpublished)
            {
                searchAccessor.RemoveRecipe(recipeId);
            }
            else
            {
                // do nothing, not related to search
            }
        }
    }


    public class RecipeSearchAccessor
    {
        //NOTE: This service would be in it's own assembly if it were real and not just demonstrative

        public record ElasticSearchConnection();

        private readonly ElasticSearchConnection connection;
        public RecipeSearchAccessor(ElasticSearchConnection connection)
        {
            this.connection = connection;
        }

        public void IndexRecipe(Recipe recipe)
        {

        }

        public void RemoveRecipe(RecipeId recipeId)
        {

        }
    } 
}
