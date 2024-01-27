using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;

public class DeleteItemCategoryCommandConverter : IToDomainConverter<Guid, DeleteItemCategoryCommand>
{
    public DeleteItemCategoryCommand ToDomain(Guid source)
    {
        return new DeleteItemCategoryCommand(new ItemCategoryId(source));
    }
}