using Clients.Web.Extensions;
using Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clients.Web.Pages.Recipes
{
    public class IndexModel : PageModel
    {
        public IReadOnlyCollection<Recipe> Recipes { get; set; } = new List<Recipe>();
        
        
        private readonly RecipeManagementService recipeManager;

        public IndexModel(RecipeManagementService recipeManager)
        {
            this.recipeManager = recipeManager;
        }

        public void OnGet()
        {
            Recipes = recipeManager.ListRecipes(User.GetUserId());
        }
    }
}


