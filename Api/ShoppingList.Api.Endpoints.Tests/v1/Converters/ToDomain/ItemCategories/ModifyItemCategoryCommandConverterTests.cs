using AutoMapper;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Commands;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Converters.ToDomain.ItemCategories;

public class ModifyItemCategoryCommandConverterTests :
    ToDomainConverterTestBase<ModifyItemCategoryContract, ModifyItemCategoryCommand, ModifyItemCategoryCommandConverter>
{
    public override ModifyItemCategoryCommandConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<ModifyItemCategoryContract, ModifyItemCategoryCommand> mapping)
    {
        mapping
            .ForCtorParam(nameof(ModifyItemCategoryCommand.Modification),
                opt => opt.MapFrom(src => new ItemCategoryModification(
                    new ItemCategoryId(src.ItemCategoryId),
                    new ItemCategoryName(src.Name))));
    }
}