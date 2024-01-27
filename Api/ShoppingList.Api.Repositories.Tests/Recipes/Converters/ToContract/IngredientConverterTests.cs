using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Converters.ToContract;
using Ingredient = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Ingredient;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Recipes.Converters.ToContract;

public class IngredientConverterTests : ToContractConverterTestBase<(RecipeId, IIngredient), Ingredient, IngredientConverter>
{
    protected override IngredientConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<(RecipeId, IIngredient), Ingredient> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Item2.Id.Value))
            .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.Item1))
            .ForMember(dest => dest.ItemCategoryId, opt => opt.MapFrom(src => src.Item2.ItemCategoryId.Value))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Item2.Quantity.Value))
            .ForMember(dest => dest.QuantityType, opt => opt.MapFrom(src => src.Item2.QuantityType.ToInt()))
            .ForMember(dest => dest.DefaultItemId, opt => opt.MapFrom(src => src.Item2.DefaultItemId!.Value))
            .ForMember(dest => dest.DefaultItemTypeId, opt => opt.MapFrom(src => src.Item2.DefaultItemTypeId!.Value))
            .ForMember(dest => dest.DefaultStoreId, opt => opt.MapFrom(src => src.Item2.ShoppingListProperties!.DefaultStoreId.Value))
            .ForMember(dest => dest.AddToShoppingListByDefault, opt => opt.MapFrom(src => src.Item2.ShoppingListProperties!.AddToShoppingListByDefault))
            .ForMember(dest => dest.Recipe, opt => opt.Ignore());
    }
}