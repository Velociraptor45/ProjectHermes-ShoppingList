using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;

public class CreateItemCategoryCommandConverter : IToDomainConverter<string, CreateItemCategoryCommand>
{
    public CreateItemCategoryCommand ToDomain(string source)
    {
        return new CreateItemCategoryCommand(new ItemCategoryName(source));
    }
}