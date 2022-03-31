using Microsoft.Extensions.DependencyInjection.Extensions;
using Managers;
using Managers.RecipeManagement;
using Managers.RecipeManagement.RecipeEventNotifiers;
using Managers.RecipeManagement.Identity;
using Clients.Web.Extensions;
using static Managers.RecipeManagement.Identity.AuthorizedRecipeManagementServiceDecorator;

namespace Clients.Web
{
#pragma warning disable CS8604 // Possible null reference argument.
    public class CompositionRoot
    {

        public static void RegisterAppServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<RequestContext>(ctx =>
            {
                var httpContext = ctx.GetService<IHttpContextAccessor>();
                return new RequestContext(httpContext?.HttpContext?.User.GetUserId());
            });
            services.AddScoped<IRecipeManagementService>(ctx =>
            {
                return new AuthorizedRecipeManagementServiceDecorator(
                    ctx.GetService<RequestContext>(),
                    new RecipeManagementService(ctx.GetService<IRecipeEventNotifier>(), ctx.GetService<IRecipeAccess>()));
            });
            services.AddSingleton<IRecipeAccess, InMemoryRecipeAccess>();
            services.AddSingleton<IRecipeEventNotifier, NullRecipeEventNotifier>();
        }
    }
#pragma warning restore CS8604 // Possible null reference argument.

}
