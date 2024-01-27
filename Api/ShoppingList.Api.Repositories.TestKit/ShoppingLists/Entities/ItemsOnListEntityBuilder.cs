using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.TestKit.ShoppingLists.Entities;

public class ItemsOnListEntityBuilder : TestBuilderBase<ItemsOnList>
{
    public ItemsOnListEntityBuilder()
    {
        WithoutShoppingList();
    }

    public ItemsOnListEntityBuilder WithId(int id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public ItemsOnListEntityBuilder WithShoppingListId(Guid shoppingListId)
    {
        FillPropertyWith(p => p.ShoppingListId, shoppingListId);
        return this;
    }

    public ItemsOnListEntityBuilder WithItemId(Guid itemId)
    {
        FillPropertyWith(p => p.ItemId, itemId);
        return this;
    }

    public ItemsOnListEntityBuilder WithItemTypeId(Guid? itemTypeId)
    {
        FillPropertyWith(p => p.ItemTypeId, itemTypeId);
        return this;
    }

    public ItemsOnListEntityBuilder WithoutItemTypeId()
    {
        return WithItemTypeId(null);
    }

    public ItemsOnListEntityBuilder WithInBasket(bool inBasket)
    {
        FillPropertyWith(p => p.InBasket, inBasket);
        return this;
    }

    public ItemsOnListEntityBuilder WithQuantity(float quantity)
    {
        FillPropertyWith(p => p.Quantity, quantity);
        return this;
    }

    public ItemsOnListEntityBuilder WithSectionId(Guid sectionId)
    {
        FillPropertyWith(p => p.SectionId, sectionId);
        return this;
    }

    public ItemsOnListEntityBuilder WithShoppingList(Repositories.ShoppingLists.Entities.ShoppingList? shoppingList)
    {
        FillPropertyWith(p => p.ShoppingList, shoppingList);
        return this;
    }

    public ItemsOnListEntityBuilder WithoutShoppingList()
    {
        return WithShoppingList(null);
    }
}