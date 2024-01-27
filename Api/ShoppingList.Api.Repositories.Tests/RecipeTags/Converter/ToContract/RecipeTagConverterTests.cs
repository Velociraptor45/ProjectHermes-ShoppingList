using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Converters.ToContract;
using RecipeTag = ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Entities.RecipeTag;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.RecipeTags.Converter.ToContract;

public class RecipeTagConverterTests : ToContractConverterTestBase<IRecipeTag, RecipeTag, RecipeTagConverter>
{
    protected override RecipeTagConverter CreateSut()
    {
        return new RecipeTagConverter();
    }

    protected override void AddMapping(IMappingExpression<IRecipeTag, RecipeTag> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => ((AggregateRoot)src).RowVersion));
    }
}