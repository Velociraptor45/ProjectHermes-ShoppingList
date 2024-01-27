using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Converters.ToContract;

public class ItemCategoryConverter : IToContractConverter<IItemCategory, Entities.ItemCategory>
{
    public Entities.ItemCategory ToContract(IItemCategory source)
    {
        return new Entities.ItemCategory
        {
            Id = source.Id,
            Name = source.Name,
            Deleted = source.IsDeleted,
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }
}