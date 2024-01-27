using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Converters.ToContract;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities;
using Ingredient = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Ingredient;
using PreparationStep = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.PreparationStep;
using Recipe = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Recipe;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Recipes.Converters.ToContract;

public class RecipeConverterTests : ToContractConverterTestBase<IRecipe, Recipe, RecipeConverter>
{
    protected override RecipeConverter CreateSut()
    {
        return new(new IngredientConverter(), new PreparationStepConverter(), new TagsForRecipeConverter());
    }

    protected override void AddMapping(IMappingExpression<IRecipe, Recipe> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.NumberOfServings, opt => opt.MapFrom(src => src.NumberOfServings.Value))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom((src, _, _, ctx) =>
                src.Ingredients.Select(i => ctx.Mapper.Map<Ingredient>((src.Id, i)))))
            .ForMember(dest => dest.PreparationSteps, opt => opt.MapFrom((src, _, _, ctx) =>
                src.PreparationSteps.Select(p => ctx.Mapper.Map<PreparationStep>((src.Id, p)))))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom((src, _, _, ctx) =>
                src.Tags.Select(t => ctx.Mapper.Map<TagsForRecipe>((src.Id, t)))))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => ((AggregateRoot)src).RowVersion));
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        new IngredientConverterTests().AddMapping(cfg);
        new PreparationStepConverterTests().AddMapping(cfg);
        new TagsForRecipeConverterTests().AddMapping(cfg);
    }
}