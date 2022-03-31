using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.RecipeManagement.Identity
{
    public class AuthorizedRecipeManagementServiceDecorator : IRecipeManagementService
    {
        private readonly RequestContext requestContext;
        private readonly IRecipeManagementService decorated;

        
        public AuthorizedRecipeManagementServiceDecorator(RequestContext requestContext, IRecipeManagementService decorated)
        {
            this.requestContext = requestContext;
            this.decorated = decorated;
        }
        public void CreateRecipe(UserId userId, Recipe recipe)
        {
            if (userId != requestContext.CurrentUserId) throw new UnauthorizedAccessException();
            
            decorated.CreateRecipe(userId, recipe);
        }

        public IReadOnlyCollection<Recipe> ListRecipes(UserId userId)
        {
            if (userId != requestContext.CurrentUserId) throw new UnauthorizedAccessException();

            return decorated.ListRecipes(userId);
        }

        public PublicationResult Publish(RecipeId recipeId)
        {
            return decorated.Publish(recipeId);
        }

        public Recipe? RecipeDetails(RecipeId recipeId)
        {
            return decorated.RecipeDetails(recipeId);
        }


        public record RequestContext(UserId? CurrentUserId);
    }
}
