using Microsoft.Extensions.DependencyInjection.Extensions;
using Managers;
using Managers.RecipeManagement;

namespace Clients.Web
{
    public class CompositionRoot
    {

        public static void RegisterAppServices(IServiceCollection services)
        {
            services.AddScoped<RecipeManagementService>();
            services.AddSingleton<IRecipeAccess, InMemoryRecipeAccess>();
            services.AddSingleton<IRecipeEventNotifier, NullRecipeEventNotifier>();
        }
    }

}
