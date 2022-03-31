using Clients.Web.Extensions;
using Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clients.Web.Pages.Recipes
{
    public class CreateRecipeModel : PageModel
    {
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string? Instructions { get; set; }



        private readonly IRecipeManagementService recipeManager;

        public CreateRecipeModel(IRecipeManagementService recipeManager)
        {
            this.recipeManager = recipeManager;
        }


        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Recipe recipe = new Recipe(RecipeId.NewId(), Title, new Markdown(Instructions));

            recipeManager.CreateRecipe(User.GetUserId(), recipe);

            return RedirectToPage("Index");
        }
    }
}
