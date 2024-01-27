using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Converters.ToContract;

public class ShoppingListConverter : IToContractConverter<IShoppingList, Entities.ShoppingList>
{
    public Entities.ShoppingList ToContract(IShoppingList source)
    {
        return new Entities.ShoppingList()
        {
            Id = source.Id,
            CompletionDate = source.CompletionDate,
            StoreId = source.StoreId,
            ItemsOnList = CreateItemsOnListMap(source).ToList(),
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }

    private IEnumerable<ItemsOnList> CreateItemsOnListMap(IShoppingList source)
    {
        foreach (var section in source.Sections)
        {
            foreach (var item in section.Items)
            {
                yield return new ItemsOnList()
                {
                    ShoppingListId = source.Id,
                    ItemId = item.Id,
                    ItemTypeId = item.TypeId,
                    InBasket = item.IsInBasket,
                    Quantity = item.Quantity.Value,
                    SectionId = section.Id
                };
            }
        }
    }
}