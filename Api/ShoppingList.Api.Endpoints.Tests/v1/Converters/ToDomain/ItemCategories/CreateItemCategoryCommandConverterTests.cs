using AutoMapper;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Converters.ToDomain.ItemCategories;

public class CreateItemCategoryCommandConverterTests :
    ToDomainConverterTestBase<string, CreateItemCategoryCommand, CreateItemCategoryCommandConverter>
{
    public override CreateItemCategoryCommandConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<string, CreateItemCategoryCommand> mapping)
    {
        mapping
            .ForCtorParam(nameof(CreateItemCategoryCommand.Name).LowerFirstChar(), opt => opt.MapFrom(src => src));
    }
}