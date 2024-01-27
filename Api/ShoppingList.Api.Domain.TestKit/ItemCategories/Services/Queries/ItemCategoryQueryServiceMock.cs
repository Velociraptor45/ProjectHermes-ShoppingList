using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.ItemCategories.Services.Queries;

public class ItemCategoryQueryServiceMock : Mock<IItemCategoryQueryService>
{
    public ItemCategoryQueryServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupGetAsync(ItemCategoryId itemCategoryId, IItemCategory returnValue)
    {
        Setup(m => m.GetAsync(itemCategoryId)).ReturnsAsync(returnValue);
    }

    public void SetupGetAsync(string searchInput, bool includeDeleted,
        IEnumerable<ItemCategorySearchResultReadModel> returnValue)
    {
        Setup(m => m.GetAsync(searchInput, includeDeleted)).ReturnsAsync(returnValue);
    }
}