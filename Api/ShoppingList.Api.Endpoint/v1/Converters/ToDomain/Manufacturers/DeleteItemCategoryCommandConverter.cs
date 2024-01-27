using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.DeleteManufacturer;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Manufacturers;

public class DeleteManufacturerCommandConverter : IToDomainConverter<Guid, DeleteManufacturerCommand>
{
    public DeleteManufacturerCommand ToDomain(Guid source)
    {
        return new DeleteManufacturerCommand(new ManufacturerId(source));
    }
}