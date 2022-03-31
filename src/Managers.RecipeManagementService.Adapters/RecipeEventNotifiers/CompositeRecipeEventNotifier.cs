using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.RecipeManagement.Adapters.RecipeEventNotifiers
{
    public class CompositeRecipeEventNotifier : IRecipeEventNotifier
    {
        private readonly IReadOnlyCollection<IRecipeEventNotifier> notifiers;

        public CompositeRecipeEventNotifier(IReadOnlyCollection<IRecipeEventNotifier> notifiers)
        {
            this.notifiers = notifiers;
        }

        public void Notify(RecipeEvent recipeEvent, RecipeId recipeId)
        {
            foreach (var notifier in notifiers)
            {
                notifier.Notify(recipeEvent, recipeId);
            }
        }
    }
}
