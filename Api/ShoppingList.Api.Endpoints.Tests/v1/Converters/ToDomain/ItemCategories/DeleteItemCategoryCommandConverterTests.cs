using AutoMapper;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Converters.ToDomain.ItemCategories;

public class DeleteItemCategoryCommandConverterTests :
    ToDomainConverterTestBase<Guid, DeleteItemCategoryCommand, DeleteItemCategoryCommandConverter>
{
    public override DeleteItemCategoryCommandConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<Guid, DeleteItemCategoryCommand> mapping)
    {
        mapping
            .ForCtorParam(nameof(DeleteItemCategoryCommand.ItemCategoryId).LowerFirstChar(),
                opt => opt.MapFrom(src => src));
    }
}