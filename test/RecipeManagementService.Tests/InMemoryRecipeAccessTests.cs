using Managers;

namespace RecipeManagementServiceTests;

public class InMemoryRecipeAccessTests : IRecipeAccessTests
{
    public override IRecipeAccess SutFactory()
    {
        return new InMemoryRecipeAccess();
    }
}