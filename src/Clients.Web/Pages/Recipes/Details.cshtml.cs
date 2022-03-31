using Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clients.Web.Pages.Recipes
{
    public class DetailsModel : PageModel
    {
        public Recipe? Recipe { get; set; }

        private readonly IRecipeManagementService recipeManager;
        public DetailsModel(IRecipeManagementService recipeManager)
        {
            this.recipeManager = recipeManager;
        }
        public void OnGet(Guid id)
        {
            var recipeId = new RecipeId(id);

            Recipe = recipeManager.RecipeDetails(recipeId);
        }
    }
}
