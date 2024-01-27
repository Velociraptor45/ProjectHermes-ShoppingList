using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.TestKit.ShoppingLists.Entities;

public class ShoppingListEntityBuilder : TestBuilderBase<Repositories.ShoppingLists.Entities.ShoppingList>
{
    public ShoppingListEntityBuilder()
    {
        WithItemsOnList(new ItemsOnListEntityBuilder().CreateMany(3).ToList());
    }

    public ShoppingListEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public ShoppingListEntityBuilder WithCompletionDate(DateTimeOffset? completionDate)
    {
        FillPropertyWith(p => p.CompletionDate, completionDate);
        return this;
    }

    public ShoppingListEntityBuilder WithoutCompletionDate()
    {
        return WithCompletionDate(null);
    }

    public ShoppingListEntityBuilder WithStoreId(Guid storeId)
    {
        FillPropertyWith(p => p.StoreId, storeId);
        return this;
    }

    public ShoppingListEntityBuilder WithItemsOnList(ICollection<ItemsOnList> itemsOnList)
    {
        FillPropertyWith(p => p.ItemsOnList, itemsOnList);
        return this;
    }

    public ShoppingListEntityBuilder WithEmptyItemsOnList()
    {
        return WithItemsOnList(new List<ItemsOnList>());
    }
}