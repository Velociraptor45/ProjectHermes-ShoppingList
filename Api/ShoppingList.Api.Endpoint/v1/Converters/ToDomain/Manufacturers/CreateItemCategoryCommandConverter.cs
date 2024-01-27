using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Manufacturers;

public class CreateManufacturerCommandConverter : IToDomainConverter<string, CreateManufacturerCommand>
{
    public CreateManufacturerCommand ToDomain(string source)
    {
        return new CreateManufacturerCommand(new ManufacturerName(source));
    }
}