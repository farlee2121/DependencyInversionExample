using Managers;
using Managers.RecipeManagement;

namespace RecipeManagementServiceTests;

public class InMemoryRecipeAccessTests : IRecipeAccessTests
{
    public override IRecipeAccess SutFactory()
    {
        return new InMemoryRecipeAccess();
    }
}