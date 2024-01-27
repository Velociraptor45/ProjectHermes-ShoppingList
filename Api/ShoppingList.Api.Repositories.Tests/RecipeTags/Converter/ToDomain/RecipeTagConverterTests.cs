using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models.Factories;
using ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;
using RecipeTag = ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Entities.RecipeTag;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.RecipeTags.Converter.ToDomain;

public class RecipeTagConverterTests : ToDomainConverterTestBase<RecipeTag, IRecipeTag, RecipeTagConverter>
{
    public override RecipeTagConverter CreateSut()
    {
        return new RecipeTagConverter(new RecipeTagFactory());
    }

    protected override void AddMapping(IMappingExpression<RecipeTag, IRecipeTag> mapping)
    {
        mapping.As<Domain.RecipeTags.Models.RecipeTag>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<RecipeTag, Domain.RecipeTags.Models.RecipeTag>()
            .ForCtorParam(nameof(IRecipeTag.Id).LowerFirstChar(), opt => opt.MapFrom(src => new RecipeTagId(src.Id)))
            .ForCtorParam(nameof(IRecipeTag.Name).LowerFirstChar(), opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());
    }
}