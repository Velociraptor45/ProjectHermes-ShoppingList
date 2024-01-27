using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Converters.ToContract;

public class ManufacturerConverter : IToContractConverter<IManufacturer, Entities.Manufacturer>
{
    public Entities.Manufacturer ToContract(IManufacturer source)
    {
        return new Entities.Manufacturer()
        {
            Id = source.Id,
            Name = source.Name,
            Deleted = source.IsDeleted,
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }
}